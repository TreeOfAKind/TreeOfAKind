import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/common/loading_indicator.dart';
import 'package:tree_of_a_kind/features/people/view/add_person_page.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';
import 'package:tree_of_a_kind/features/tree/view/tree_graph_view.dart';

class TreePage extends StatefulWidget {
  const TreePage({Key key, @required this.treeItem}) : super(key: key);

  final TreeItemDTO treeItem;

  @override
  _TreePageState createState() => _TreePageState(treeItem);

  static Route route(TreeItemDTO treeItem) {
    return MaterialPageRoute<void>(
      builder: (context) => BlocProvider<TreeBloc>(
        create: (context) => TreeBloc(
            treeRepository: RepositoryProvider.of<TreeRepository>(context),
            peopleRepository: RepositoryProvider.of<PeopleRepository>(context))
          ..add(FetchTree(treeItem.id)),
        child: TreePage(treeItem: treeItem),
      ),
    );
  }
}

class _TreePageState extends State<TreePage> {
  _TreePageState(this.treeItem);

  final TreeItemDTO treeItem;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
        appBar: AppBar(title: Text(treeItem.treeName)),
        floatingActionButton: FloatingActionButton(
          child: Icon(Icons.add),
          onPressed: () {
            // ignore: close_sinks
            final bloc = BlocProvider.of<TreeBloc>(context);
            if (bloc.state is PresentingTree) {
              Navigator.of(context).push<void>(AddPersonPage.route(
                  bloc, (bloc.state as PresentingTree).tree));
            }
          },
        ),
        body: BlocBuilder<TreeBloc, TreeState>(
          builder: (context, state) {
            if (state is LoadingState) {
              return Center(child: LoadingIndicator());
            } else if (state is UnknownErrorState) {
              return GenericError();
            } else if (state is PresentingTree) {
              return state.treeGraph.isEmpty
                  ? Center(
                      child: Padding(
                      padding: const EdgeInsets.all(20.0),
                      child: Text(
                        "Your family members will be here, once you add them ☺",
                        style: theme.textTheme.headline3,
                        textAlign: TextAlign.center,
                      ),
                    ))
                  : TreeGraphView(tree: state.tree, treeGraph: state.treeGraph);
            } else {
              return Container();
            }
          },
        ));
  }
}
