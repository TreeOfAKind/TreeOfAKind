import 'package:charts_flutter/flutter.dart' as charts;
import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/stats/view/pie_chart.dart';

class StatsView extends StatelessWidget {
  const StatsView({Key key, @required this.stats}) : super(key: key);

  final TreeStatsDTO stats;

  static const List<String> _genders = ["Unknown", "Male", "Female", "Other"];

  static Color _genderToColor(String gender) {
    switch (gender) {
      case "Unknown":
        return Colors.indigo;
      case "Male":
        return Colors.brown;
      case "Female":
        return Colors.lightGreen;
      case "Other":
        return Colors.amber;
      default:
        return Colors.indigo;
    }
  }

  charts.Series<_StringIntColorStats, int> generateGenderSeries(
      BuildContext context) {
    final data = List<_StringIntColorStats>();
    stats.numberOfPeopleOfEachGender.forEach((gender, number) =>
        data.add(_StringIntColorStats(gender, number, _genderToColor(gender))));

    return charts.Series<_StringIntColorStats, int>(
      id: "Gender",
      domainFn: (s, __) => _genders.indexOf(s.text),
      measureFn: (s, __) => s.value,
      data: data,
      colorFn: (s, __) => charts.ColorUtil.fromDartColor(s.color),
      labelAccessorFn: (s, __) => '${s.text}: ${s.value}',
    );
  }

  charts.Series<_StringIntColorStats, int> generateLivingDeadSeries(
      BuildContext context) {
    final livingStats = _StringIntColorStats(
        "Living", stats.numberOfLivingPeople, Colors.lightGreen);
    final deadStats = _StringIntColorStats(
        "Deceased", stats.numberOfDeceasedPeople, Colors.brown);

    return charts.Series<_StringIntColorStats, int>(
      id: "Living/Dead",
      domainFn: (s, __) => s == livingStats ? 0 : 1,
      measureFn: (s, __) => s.value,
      data: [livingStats, deadStats],
      colorFn: (s, __) => charts.ColorUtil.fromDartColor(s.color),
      labelAccessorFn: (s, __) => '${s.text}: ${s.value}',
    );
  }

  charts.Series<_StringIntColorStats, int> generateMarriedSingleSeries(
      BuildContext context) {
    final marriedStats = _StringIntColorStats(
        "Married", stats.numberOfMarriedPeople, Colors.lightGreen);
    final singleStats = _StringIntColorStats(
        "Single", stats.numberOfSinglePeople, Colors.brown);

    return charts.Series<_StringIntColorStats, int>(
      id: "Married/Single",
      domainFn: (s, __) => s == marriedStats ? 0 : 1,
      measureFn: (s, __) => s.value,
      data: [marriedStats, singleStats],
      colorFn: (s, __) => charts.ColorUtil.fromDartColor(s.color),
      labelAccessorFn: (s, __) => '${s.text}: ${s.value}',
    );
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return SingleChildScrollView(
        child: Padding(
      padding: const EdgeInsets.all(16.0),
      child: Column(
        children: [
          Text(
            "Here are various curiosities related to this family ☺",
            style: theme.textTheme.headline5,
            textAlign: TextAlign.center,
          ),
          const Divider(),
          const StatsLabel("Family members total count:"),
          const SizedBox(height: 8.0),
          Text(
            "${stats.totalNumberOfPeople}",
            style: theme.textTheme.headline1,
            textAlign: TextAlign.center,
          ),
          const Divider(),
          LabeledPieChart(
              label: "Living and dead family members counts:",
              series: generateLivingDeadSeries(context)),
          if (stats.averageLifespanInDays != 0) ...[
            const Divider(),
            const StatsLabel("Family members average lifespan:"),
            Text(
              "${(stats.averageLifespanInDays / 365.25).toStringAsFixed(2)} yrs.",
              style: theme.textTheme.headline2,
              textAlign: TextAlign.center,
            ),
          ],
          const Divider(),
          LabeledPieChart(
              label: "Family members genders counts:",
              series: generateGenderSeries(context)),
          const Divider(),
          const StatsLabel("Family members average count of children:"),
          Text(
            "About ${stats.averageNumberOfChildren.toStringAsFixed(2)} children",
            style: theme.textTheme.headline2,
            textAlign: TextAlign.center,
          ),
          const Divider(),
          LabeledPieChart(
              label: "Married and single family members counts:",
              series: generateMarriedSingleSeries(context)),
        ],
      ),
    ));
  }
}

class StatsLabel extends StatelessWidget {
  const StatsLabel(this.label, {Key key}) : super(key: key);

  final String label;

  @override
  Widget build(BuildContext context) {
    return Text(
      label,
      style: Theme.of(context).textTheme.subtitle1,
      textAlign: TextAlign.left,
    );
  }
}

class LabeledPieChart extends StatelessWidget {
  final String label;
  final charts.Series series;

  const LabeledPieChart({Key key, this.label, this.series}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Column(children: [
      StatsLabel(label),
      const SizedBox(height: 20.0),
      ConstrainedBox(
          constraints: BoxConstraints.expand(height: 200.0),
          child: PieChart([series]))
    ]);
  }
}

class _StringIntColorStats {
  _StringIntColorStats(this.text, this.value, this.color);

  final String text;
  final int value;
  final Color color;
}
