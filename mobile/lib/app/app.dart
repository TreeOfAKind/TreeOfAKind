import 'package:authentication_repository/authentication_repository.dart';
import 'package:connectivity/connectivity.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/features/common/no_internet_page.dart';
import 'package:tree_of_a_kind/features/common/splash_page.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:tree_of_a_kind/features/home/home.dart';
import 'package:tree_of_a_kind/features/login/login.dart';
import 'package:tree_of_a_kind/resources/theme.dart';

class App extends StatefulWidget {
  const App({Key key, this.connectedAtStart}) : super(key: key);

  final bool connectedAtStart;

  @override
  _AppState createState() => _AppState(connectedAtStart);
}

class _AppState extends State<App> {
  _AppState(this.connected);

  final _navigatorKey = GlobalKey<NavigatorState>();

  NavigatorState get _navigator => _navigatorKey.currentState;

  bool connected;

  @override
  initState() {
    super.initState();

    Connectivity().onConnectivityChanged.listen((ConnectivityResult result) {
      setState(() => connected = result != ConnectivityResult.none);
    });
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      theme: theme,
      navigatorKey: _navigatorKey,
      builder: (context, child) {
        return BlocListener<AuthenticationBloc, AuthenticationState>(
          listener: (context, state) {
            if (!connected) {
              _navigator.pushAndRemoveUntil<void>(
                NoInternetPage.route(),
                (route) => false,
              );
            }

            switch (state.status) {
              case AuthenticationStatus.authenticated:
                _navigator.pushAndRemoveUntil<void>(
                  HomePage.route(),
                  (route) => false,
                );
                break;
              case AuthenticationStatus.unauthenticated:
                _navigator.pushAndRemoveUntil<void>(
                  LoginPage.route(),
                  (route) => false,
                );
                break;
              default:
                break;
            }
          },
          child: child,
        );
      },
      onGenerateRoute: (_) => SplashPage.route(),
    );
  }
}
