import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/resources/app_colors.dart';

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
                  children: [
                    TextSpan(
                      text: "An error has occured ",
                    ),
                    WidgetSpan(
                      child: Icon(
                        Icons.error,
                        color: AppColors.white,
                      ),
                    ),
                  ],
                ),
              ))),
    );
  }
}
