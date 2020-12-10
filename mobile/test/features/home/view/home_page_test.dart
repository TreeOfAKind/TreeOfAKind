import 'package:bloc_test/bloc_test.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:tree_of_a_kind/features/home/bloc/tree_list_bloc.dart';
import 'package:tree_of_a_kind/features/home/home.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mockito/mockito.dart';

class MockAuthenticationBloc extends MockBloc<AuthenticationState>
    implements AuthenticationBloc {}

class MockTreeListBloc extends MockBloc<TreeListState> implements TreeListBloc {
}

// ignore: must_be_immutable
class MockUser extends Mock implements User {
  @override
  String get email => 'test@gmail.com';
}

void main() {
  const menuButtonKey = Key('homePage_menu_popupMenuButton');

  group('HomePage', () {
    // ignore: close_sinks
    AuthenticationBloc authenticationBloc;
    // ignore: close_sinks
    TreeListBloc treeListBloc;
    User user;
    TreeItemDTO treeItem;

    setUp(() {
      authenticationBloc = MockAuthenticationBloc();
      user = MockUser();
      when(authenticationBloc.state).thenReturn(
        AuthenticationState.authenticated(user),
      );

      treeListBloc = MockTreeListBloc();
      treeItem = TreeItemDTO(id: "id_1", treeName: "Test tree");
      when(treeListBloc.state).thenReturn(
        PresentingList([treeItem]),
      );
    });

    group('renders', () {
      testWidgets('menu button', (tester) async {
        await tester.pumpWidget(
          MultiBlocProvider(
            providers: [
              BlocProvider.value(
                value: authenticationBloc,
              ),
              BlocProvider.value(
                value: treeListBloc,
              ),
            ],
            child: MaterialApp(
              home: HomePage(),
            ),
          ),
        );

        expect(find.byKey(menuButtonKey), findsOneWidget);
      });
    });
  });
}
