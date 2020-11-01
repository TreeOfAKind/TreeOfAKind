import 'package:authentication_repository/authentication_repository.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:tree_of_a_kind/features/home/home.dart';

class HomePage extends StatefulWidget {
  HomePage({Key key}) : super(key: key);

  @override
  _HomePageState createState() => _HomePageState();

  static Route route() {
    return MaterialPageRoute<void>(builder: (_) => HomePage());
  }
}

class _HomePageState extends State<HomePage> {
  int _selectedIndex = 2;
  static const TextStyle optionStyle =
      TextStyle(fontSize: 30, fontWeight: FontWeight.bold);
  static List<Widget> _widgetOptions(
    User user,
    TextTheme textTheme,
  ) {
    return <Widget>[
      Center(
          child: Text(
        'Index 0: Home',
        style: optionStyle,
      )),
      Center(
          child: Text(
        'Index 1: Family',
        style: optionStyle,
      )),
      Align(
        alignment: const Alignment(0, -1 / 3),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: <Widget>[
            Avatar(photo: user.photoURL),
            const SizedBox(height: 4.0),
            Text(user.email, style: textTheme.headline6),
            const SizedBox(height: 4.0),
            Text(user.displayName ?? '', style: textTheme.headline5),
          ],
        ),
      ),
    ];
  }

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });
  }

  @override
  Widget build(BuildContext context) {
    final textTheme = Theme.of(context).textTheme;
    final user = context.bloc<AuthenticationBloc>().state.user;
    return Scaffold(
      appBar: AppBar(
        title: const Text('Home'),
        actions: <Widget>[
          IconButton(
            key: const Key('homePage_logout_iconButton'),
            icon: const Icon(Icons.exit_to_app),
            onPressed: () => context
                .bloc<AuthenticationBloc>()
                .add(AuthenticationLogoutRequested()),
          )
        ],
      ),
      body: _widgetOptions(user, textTheme).elementAt(_selectedIndex),
      bottomNavigationBar: BottomNavigationBar(
        items: const <BottomNavigationBarItem>[
          BottomNavigationBarItem(
            icon: Icon(Icons.home),
            label: 'Home',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.nature_people),
            label: 'Family',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.account_circle),
            label: 'My Profile',
          ),
        ],
        currentIndex: _selectedIndex,
        selectedItemColor: Theme.of(context).primaryColor,
        onTap: _onItemTapped,
      ),
    );
  }
}
