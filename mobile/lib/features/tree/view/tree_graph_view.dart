import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:graphite/core/matrix.dart';
import 'package:graphite/graphite.dart';
import 'package:random_color/random_color.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/common/avatar.dart';
import 'package:tree_of_a_kind/features/people/view/update_person_page.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class TreeGraphView extends StatelessWidget {
  TreeGraphView({Key key, @required this.tree, @required this.treeGraph})
      : randomColors = RandomColor(tree.treeId.codeUnits
            .fold(0, (previousValue, element) => previousValue + element)),
        super(key: key);

  final TreeDTO tree;
  final List<NodeInput> treeGraph;

  final RandomColor randomColors;

  @override
  Widget build(BuildContext context) {
    // ignore: close_sinks
    return DirectGraph(
      list: treeGraph,
      cellWidth: 140.0,
      cellPadding: 20.0,
      orientation: MatrixOrientation.Vertical,
      onNodeLongPressStart: (_, node) => Navigator.of(context).push<void>(
          UpdatePersonPage.route(BlocProvider.of<TreeBloc>(context), tree,
              tree.people.firstWhere((person) => person.id == node.id))),
      builder: (context, node) => _PersonNode(
          person: tree.people.firstWhere((person) => person.id == node.id),
          color: randomColors.randomColor(
              colorHue: ColorHue.blue,
              colorSaturation: ColorSaturation.lowSaturation)),
    );
  }
}

class _PersonNode extends StatelessWidget {
  const _PersonNode({Key key, @required this.person, @required this.color})
      : super(key: key);

  static const _avatarSize = 24.0;

  final PersonDTO person;
  final Color color;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Column(children: [
      SizedBox(height: 8.0),
      Avatar(
          photo: person.mainPhoto?.uri,
          backgroundColor: color,
          avatarSize: _avatarSize),
      SizedBox(height: 4.0),
      Text(
        person.name,
        textAlign: TextAlign.center,
        overflow: TextOverflow.ellipsis,
      ),
      Text(
        person.lastName,
        textAlign: TextAlign.center,
        overflow: TextOverflow.ellipsis,
      ),
    ]);
  }
}
