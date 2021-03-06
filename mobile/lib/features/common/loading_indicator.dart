import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/resources/app_colors.dart';

class LoadingIndicator extends StatelessWidget {
  const LoadingIndicator({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return CircularProgressIndicator(
      valueColor: AlwaysStoppedAnimation<Color>(AppColors.main),
    );
  }
}
