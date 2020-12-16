import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:progress_indicators/progress_indicators.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/home/bloc/tree_list_bloc.dart';
import 'package:tree_of_a_kind/features/tree/view/tree_page.dart';

class TreeListView extends StatelessWidget {
  TreeListView(
      {@required this.treeList, this.isRefreshing = false, this.deletedTreeId});

  final List<TreeItemDTO> treeList;
  final bool isRefreshing;
  final String deletedTreeId;

  @override
  Widget build(BuildContext context) {
    return ListView(children: [
      ...treeList.map((item) => item.id != deletedTreeId
          ? _TreeItem(treeItem: item)
          : _TreeItemLoading()),
      if (isRefreshing && deletedTreeId == null) _TreeItemLoading()
    ]);
  }
}

class _TreeItem extends StatelessWidget {
  _TreeItem({@required this.treeItem});

  final TreeItemDTO treeItem;

  void _listTap(BuildContext context) {
    Navigator.of(context).push<void>(TreePage.route(treeItem));
  }

  void _deleteTreeDialog(BuildContext context, TreeListBloc bloc) {
    final theme = Theme.of(context);

    showDialog(
        context: context,
        builder: (context) => new AlertDialog(
              title: Text('Are you sure?'),
              titleTextStyle: theme.textTheme.headline4,
              content: Text('Do you want to delete tree ${treeItem.treeName}?',
                  style: theme.textTheme.bodyText1),
              actions: <Widget>[
                TextButton(
                  child: Text('No'),
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                ),
                TextButton(
                  child: Text('Yes'),
                  onPressed: () {
                    Navigator.of(context).pop();
                    bloc.add(DeleteTree(treeItem.id));
                  },
                ),
              ],
            ));
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Slidable(
        actionPane: SlidableDrawerActionPane(),
        child: Card(
            shadowColor: theme.shadowColor,
            child: ListTile(
              leading: Icon(
                Icons.nature,
                color: theme.accentColor,
              ),
              title: Text(treeItem.treeName, style: theme.textTheme.headline6),
              onTap: () => _listTap(context),
              trailing: Icon(Icons.keyboard_arrow_right),
            )),
        secondaryActions: [
          IconSlideAction(
            caption: 'Delete',
            color: theme.errorColor,
            icon: Icons.delete,
            onTap: () => _deleteTreeDialog(
                context, BlocProvider.of<TreeListBloc>(context)),
          ),
        ]);
  }
}

class _TreeItemLoading extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Card(
        shadowColor: theme.shadowColor,
        child: ListTile(
          leading: Icon(
            Icons.nature,
            color: theme.accentColor,
          ),
          title: JumpingDotsProgressIndicator(
            fontSize: theme.textTheme.headline6.fontSize,
          ),
        ));
  }
}
