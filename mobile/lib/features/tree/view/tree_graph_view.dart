import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:graphview/GraphView.dart';
import 'package:random_color/random_color.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/common/avatar.dart';
import 'package:tree_of_a_kind/features/people/view/update_person_page.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class TreeGraphView extends StatelessWidget {
  TreeGraphView({Key key, @required this.tree})
      : randomColors = RandomColor(tree.treeId.codeUnits
            .fold(0, (previousValue, element) => previousValue + element)),
        super(key: key);

  final TreeDTO tree;
  final RandomColor randomColors;

  final _config = SugiyamaConfiguration()
    ..nodeSeparation = (50)
    ..levelSeparation = (80);

  Graph _buildGraph() {
    final graph = Graph();

    final nodes = tree.people
        .map((person) => Node(_PersonNode(
            person: person,
            tree: tree,
            color: randomColors.randomColor(
                colorHue: ColorHue.green,
                colorSaturation: ColorSaturation.lowSaturation))))
        .toList();

    graph.addNodes(nodes);

    var personIdx = 0;
    tree.people.forEach((person) {
      if (person.father != null) {
        graph.addEdge(
            nodes[tree.people.indexWhere((p) => p.id == person.father)],
            nodes[personIdx],
            paint: Paint()..color = Colors.brown);
      }

      if (person.mother != null) {
        graph.addEdge(
            nodes[tree.people.indexWhere((p) => p.id == person.mother)],
            nodes[personIdx],
            paint: Paint()..color = Colors.green);
      }

      personIdx++;
    });

    return graph;
  }

  @override
  Widget build(BuildContext context) {
    return InteractiveViewer(
      constrained: false,
      boundaryMargin: const EdgeInsets.all(double.infinity),
      minScale: 0.25,
      maxScale: 2.5,
      child: GraphView(
          graph: _buildGraph(), algorithm: SugiyamaAlgorithm(_config)),
    );
  }
}

class _PersonNode extends StatelessWidget {
  const _PersonNode(
      {Key key,
      @required this.person,
      @required this.tree,
      @required this.color})
      : super(key: key);

  static const _avatarSize = 28.0;

  final PersonDTO person;
  final TreeDTO tree;
  final Color color;

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        SizedBox(height: 8.0),
        InkWell(
            onLongPress: () => Navigator.of(context).push(
                UpdatePersonPage.route(
                    BlocProvider.of<TreeBloc>(context), tree, person)),
            child: Avatar(
                photo: person.mainPhoto?.uri,
                backgroundColor: color,
                avatarSize: _avatarSize)),
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
      ],
    );
  }
}
