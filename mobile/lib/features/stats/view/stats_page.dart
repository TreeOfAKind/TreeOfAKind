import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/features/stats/view/stats_view.dart';

class StatsPage extends StatefulWidget {
  const StatsPage({Key key}) : super(key: key);

  @override
  _StatsPageState createState() => _StatsPageState();

  static Route route(String treeId) {
    return MaterialPageRoute<void>(
      builder: (context) => StatsPage(),
    );
  }
}

class _StatsPageState extends State<StatsPage> {
  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
        appBar: AppBar(title: Text("Family tree stats")), body: StatsView());
  }
}
