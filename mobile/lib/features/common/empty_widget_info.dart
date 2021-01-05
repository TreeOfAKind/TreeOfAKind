import 'package:flutter/material.dart';

class EmptyWidgetInfo extends StatelessWidget {
  const EmptyWidgetInfo(this.info, {Key key}) : super(key: key);

  final String info;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Center(
        child: Padding(
      padding: const EdgeInsets.all(20.0),
      child: Text(
        info,
        style: theme.textTheme.headline3,
        textAlign: TextAlign.center,
      ),
    ));
  }
}
