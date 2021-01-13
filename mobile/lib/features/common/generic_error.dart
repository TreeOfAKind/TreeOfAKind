import 'package:flutter/material.dart';

class GenericError extends StatelessWidget {
  const GenericError({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(8),
      child: Center(
          child: new Directionality(
              textDirection: TextDirection.ltr,
              child: RichText(
                text: TextSpan(
                  style: TextStyle(color: Colors.black),
                  children: [
                    TextSpan(
                      text: "An error has occured ",
                    ),
                    WidgetSpan(
                      child: Icon(
                        Icons.error,
                      ),
                    ),
                  ],
                ),
              ))),
    );
  }
}
