import 'package:bloc_test/bloc_test.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:tree_of_a_kind/features/home/home.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mockito/mockito.dart';

class MockAuthenticationBloc extends MockBloc<AuthenticationState>
    implements AuthenticationBloc {}

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
    User user;

    setUp(() {
      authenticationBloc = MockAuthenticationBloc();
      user = MockUser();
      when(authenticationBloc.state).thenReturn(
        AuthenticationState.authenticated(user),
      );
    });

    group('renders', () {
      testWidgets('menu button', (tester) async {
        await tester.pumpWidget(
          BlocProvider.value(
            value: authenticationBloc,
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
