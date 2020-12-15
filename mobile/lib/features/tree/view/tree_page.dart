import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/common/loading_indicator.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class TreePage extends StatefulWidget {
  const TreePage({Key key, @required this.treeItem}) : super(key: key);

  final TreeItemDTO treeItem;

  @override
  _TreePageState createState() => _TreePageState(treeItem);

  static Route route(TreeItemDTO treeItem) {
    return MaterialPageRoute<void>(
      builder: (context) => BlocProvider<TreeBloc>(
        create: (context) => TreeBloc(
            treeRepository: RepositoryProvider.of<TreeRepository>(context))
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
    return Scaffold(
        appBar: AppBar(title: Text(treeItem.treeName)),
        body: BlocBuilder<TreeBloc, TreeState>(
          builder: (context, state) {
            if (state is InitialLoadingState) {
              return Center(child: LoadingIndicator());
            } else if (state is UnknownErrorState) {
              return GenericError();
            } else if (state is PresentingTree) {
              return Container();
            } else {
              return Container();
            }
          },
        ));
  }
}
