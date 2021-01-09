import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/features/stats/view/pie_chart.dart';

class StatsView extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return PieChart.withSampleData();
  }
}
