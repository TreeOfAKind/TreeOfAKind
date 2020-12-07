import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';

class TreeListView extends StatelessWidget {
  TreeListView({@required this.treeList});

  final List<TreeItemDTO> treeList;

  @override
  Widget build(BuildContext context) {
    return ListView(
        children: treeList.map((item) => _TreeItem(treeItem: item)).toList());
  }
}

class _TreeItem extends StatelessWidget {
  _TreeItem({@required this.treeItem});

  final TreeItemDTO treeItem;

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
          title: Text(treeItem.treeName, style: theme.textTheme.headline6),
          trailing: Icon(Icons.keyboard_arrow_right),
        ));
  }
}
