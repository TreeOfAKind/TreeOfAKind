import 'package:flutter/material.dart';

class NoInternetPage extends StatelessWidget {
  const NoInternetPage({Key key}) : super(key: key);

  static Route route() {
    return MaterialPageRoute<void>(builder: (context) => NoInternetPage());
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            leading: const Icon(Icons.nature_people),
            title: const Text('My family trees')),
        body: Padding(
          padding: const EdgeInsets.all(8),
          child: Center(
              child: new Directionality(
                  textDirection: TextDirection.ltr,
                  child: RichText(
                    text: TextSpan(
                      style: TextStyle(color: Colors.black),
                      children: [
                        TextSpan(
                          text: "App requires internet connection ",
                        ),
                        WidgetSpan(
                          child: Icon(
                            Icons.error,
                          ),
                        ),
                      ],
                    ),
                  ))),
        ));
  }
}
