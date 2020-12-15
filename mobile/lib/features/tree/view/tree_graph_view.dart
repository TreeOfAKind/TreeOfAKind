import 'package:flutter/material.dart';
import 'package:graphite/core/matrix.dart';
import 'package:graphite/graphite.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';

class TreeGraphView extends StatelessWidget {
  const TreeGraphView({Key key, @required this.tree, @required this.treeGraph})
      : super(key: key);

  final TreeDTO tree;
  final List<NodeInput> treeGraph;

  @override
  Widget build(BuildContext context) {
    return DirectGraph(
      list: treeGraph,
      cellWidth: 136.0,
      cellPadding: 24.0,
      orientation: MatrixOrientation.Vertical,
    );
  }
}
