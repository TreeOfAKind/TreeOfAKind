import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/features/common/empty_widget_info.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/common/loading_indicator.dart';
import 'package:tree_of_a_kind/features/home/bloc/tree_list_bloc.dart'
    as treeListBloc;
import 'package:tree_of_a_kind/features/owners/view/owners_page.dart';
import 'package:tree_of_a_kind/features/people/view/add_person_page.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';
import 'package:tree_of_a_kind/features/tree/view/tree_graph_view.dart';

class TreePage extends StatefulWidget {
  const TreePage({Key key, @required this.treeItem}) : super(key: key);

  final TreeItemDTO treeItem;

  @override
  _TreePageState createState() => _TreePageState();

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
  static const String _owners = 'Manage sharing';
  static const String _deleteTree = 'Delete family tree';

  static const List<String> _menuItems = <String>[_owners, _deleteTree];

  void _menuAction(String item, BuildContext context) {
    if (item == _owners) {
      if (tree != null) {
        Navigator.of(context).push<void>(OwnersPage.route(tree));
      }
    } else if (item == _deleteTree) {
      Navigator.of(context).pop(treeListBloc.DeleteTree(widget.treeItem.id));
    }
  }

  TreeDTO tree;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
        appBar: AppBar(
          title: Text(widget.treeItem.treeName),
          actions: <Widget>[
            PopupMenuButton(
              icon: const Icon(Icons.more_vert),
              onSelected: (item) => _menuAction(item, context),
              itemBuilder: (context) => _menuItems
                  .map((item) => PopupMenuItem(
                      value: item,
                      child: Text(item,
                          style: TextStyle(
                              color: item != _deleteTree
                                  ? theme.textTheme.bodyText1.color
                                  : theme.errorColor))))
                  .toList(),
            )
          ],
        ),
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
              tree = state.tree;
              return state.treeGraph.isEmpty
                  ? const EmptyWidgetInfo(
                      "Family members of this tree will be here, once you add them ☺")
                  : TreeGraphView(tree: state.tree, treeGraph: state.treeGraph);
            } else {
              return Container();
            }
          },
        ));
  }
}
