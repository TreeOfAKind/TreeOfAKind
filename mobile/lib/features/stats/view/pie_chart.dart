import 'package:charts_flutter/flutter.dart' as charts;
import 'package:flutter/material.dart';

class PieChart extends StatelessWidget {
  final List<charts.Series> seriesList;

  PieChart(this.seriesList);

  @override
  Widget build(BuildContext context) {
    return charts.PieChart(seriesList,
        animate: true,
        defaultRenderer: charts.ArcRendererConfig(arcRendererDecorators: [
          charts.ArcLabelDecorator(labelPosition: charts.ArcLabelPosition.auto)
        ]));
  }
}
