import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:tree_of_a_kind/resources/images.dart';

class SplashPage extends StatelessWidget {
  static Route route() {
    return MaterialPageRoute<void>(builder: (_) => SplashPage());
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: SvgPicture.asset(
          Images.logoWithoutName,
        ),
      ),
    );
  }
}
