import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/common/loading_indicator.dart';
import 'package:tree_of_a_kind/features/home/bloc/tree_list_bloc.dart';
import 'package:tree_of_a_kind/features/user_profile/view/user_profile_page.dart';

import 'tree_list_view.dart';

class HomePage extends StatefulWidget {
  HomePage({Key key}) : super(key: key);

  @override
  _HomePageState createState() => _HomePageState();

  static Route route() {
    return MaterialPageRoute<void>(
        builder: (context) => BlocProvider<TreeListBloc>(
              create: (context) => TreeListBloc(
                  treeRepository:
                      RepositoryProvider.of<TreeRepository>(context))
                ..add(const FetchTreeList()),
              child: HomePage(),
            ));
  }
}

class _HomePageState extends State<HomePage> {
  static const String _myProfile = 'My profile';
  static const String _signOut = 'Sign out';

  static const List<String> _menuItems = <String>[_myProfile, _signOut];

  void _menuAction(String item, BuildContext context) {
    if (item == _myProfile) {
      Navigator.of(context).push<void>(UserProfilePage.route());
    } else if (item == _signOut) {
      context.read<AuthenticationBloc>().add(AuthenticationLogoutRequested());
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          leading: const Icon(Icons.nature_people),
          title: const Text('My family trees'),
          actions: <Widget>[
            PopupMenuButton(
              key: const Key('homePage_menu_popupMenuButton'),
              icon: const Icon(Icons.more_vert),
              onSelected: (item) => _menuAction(item, context),
              itemBuilder: (context) => _menuItems
                  .map((item) => PopupMenuItem(value: item, child: Text(item)))
                  .toList(),
            )
          ],
        ),
        body: BlocBuilder<TreeListBloc, TreeListState>(
          builder: (context, state) {
            if (state is LoadingState) {
              return Center(child: LoadingIndicator());
            } else if (state is UnknownErrorState) {
              return GenericError();
            } else if (state is PresentingData) {
              return TreeListView(treeList: state.treeList);
            } else {
              return Container();
            }
          },
        ));
  }
}
