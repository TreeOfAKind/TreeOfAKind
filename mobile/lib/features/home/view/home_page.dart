import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/user_profile/user_profile_repository.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:tree_of_a_kind/features/home/home.dart';
import 'package:tree_of_a_kind/features/user_profile/bloc/user_profile_bloc.dart';
import 'package:tree_of_a_kind/features/user_profile/view/user_profile_page.dart';

class HomePage extends StatefulWidget {
  HomePage({Key key}) : super(key: key);

  @override
  _HomePageState createState() => _HomePageState();

  static Route route() {
    return MaterialPageRoute<void>(builder: (_) => HomePage());
  }
}

class _HomePageState extends State<HomePage> {
  final _navigatorKey = GlobalKey<NavigatorState>();

  NavigatorState get _navigator => _navigatorKey.currentState;

  int _selectedIndex = 2;
  static const TextStyle optionStyle =
      TextStyle(fontSize: 30, fontWeight: FontWeight.bold);

  List<Route<dynamic>> _routes = [
    HomePage.route(),
    HomePage.route(),
    UserProfilePage.route(),
  ];

  List<Widget> _widgetOptions(
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
      BlocProvider<UserProfileBloc>(
        create: (context) => UserProfileBloc(
            userProfileRepository:
                RepositoryProvider.of<UserProfileRepository>(context))
          ..add(const FetchUserProfile()),
        child: UserProfilePage(),
      ),
    ];
  }

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });

    // Navigator.of(context).pushReplacement(_routes[index]);
  }

  @override
  Widget build(BuildContext context) {
    final textTheme = Theme.of(context).textTheme;
    final user = BlocProvider.of<AuthenticationBloc>(context).state.user;
    return Scaffold(
      appBar: AppBar(
        title: const Text('Home'),
        actions: <Widget>[
          IconButton(
            key: const Key('homePage_logout_iconButton'),
            icon: const Icon(Icons.exit_to_app),
            onPressed: () => context
                .watch<AuthenticationBloc>()
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
