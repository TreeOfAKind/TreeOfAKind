import 'package:bloc_test/bloc_test.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mockito/mockito.dart';
import 'package:tree_of_a_kind/features/user_profile/bloc/user_profile_bloc.dart';
import 'package:tree_of_a_kind/features/user_profile/view/avatar.dart';
import 'package:tree_of_a_kind/features/user_profile/view/user_profile_page.dart';

class MockAuthenticationBloc extends MockBloc<AuthenticationState>
    implements AuthenticationBloc {}

class MockUserProfileBloc extends MockBloc<UserProfileState>
    implements UserProfileBloc {}

// ignore: must_be_immutable
class MockUser extends Mock implements User {
  @override
  String get email => 'test@gmail.com';
}

void main() {
  const firstnameTextFormFieldKey = Key('userProfile_firstname_textFormField');
  const lastnameTextFormFieldKey = Key('userProfile_lastname_textFormField');
  const saveRaisedButtonKey = Key('userProfile_save_raisedButton');

  group('UserProfilePage', () {
    // ignore: close_sinks
    AuthenticationBloc authenticationBloc;
    // ignore: close_sinks
    UserProfileBloc userProfileBloc;
    User user;
    UserProfileDTO userProfile;

    setUp(() {
      authenticationBloc = MockAuthenticationBloc();
      user = MockUser();
      when(authenticationBloc.state).thenReturn(
        AuthenticationState.authenticated(user),
      );

      userProfileBloc = MockUserProfileBloc();
      userProfile = UserProfileDTO(
          id: "id_1",
          firstName: "Testy",
          lastName: "Test",
          birthDate: DateTime(1998, 1, 1));
      when(userProfileBloc.state).thenReturn(
        PresentingData(true, userProfile),
      );
    });

    group('calls', () {
      final text = 'text';

      testWidgets('UserProfileFieldChanged when firstname field changes',
          (tester) async {
        await tester.pumpWidget(MultiBlocProvider(providers: [
          BlocProvider.value(
            value: authenticationBloc,
          ),
          BlocProvider.value(
            value: userProfileBloc,
          ),
        ], child: MaterialApp(home: Scaffold(body: UserProfilePage()))));

        await tester.enterText(find.byKey(firstnameTextFormFieldKey), text);

        verify(userProfileBloc.add(argThat(isA<UserProfileFieldChanged>())))
            .called(1);
      });

      testWidgets('UserProfileFieldChanged when lastname field changes',
          (tester) async {
        await tester.pumpWidget(MultiBlocProvider(providers: [
          BlocProvider.value(
            value: authenticationBloc,
          ),
          BlocProvider.value(
            value: userProfileBloc,
          ),
        ], child: MaterialApp(home: Scaffold(body: UserProfilePage()))));

        await tester.enterText(find.byKey(lastnameTextFormFieldKey), text);

        verify(userProfileBloc.add(argThat(isA<UserProfileFieldChanged>())))
            .called(1);
      });

      testWidgets('SaveButtonClicked when save button is tapped',
          (tester) async {
        await tester.pumpWidget(MultiBlocProvider(providers: [
          BlocProvider.value(
            value: authenticationBloc,
          ),
          BlocProvider.value(
            value: userProfileBloc,
          ),
        ], child: MaterialApp(home: Scaffold(body: UserProfilePage()))));

        await tester.tap(find.byKey(saveRaisedButtonKey));

        verify(userProfileBloc.add(argThat(isA<SaveButtonClicked>())))
            .called(1);
      });
    });

    group('renders', () {
      testWidgets('avatar widget', (tester) async {
        await tester.pumpWidget(MultiBlocProvider(providers: [
          BlocProvider.value(
            value: authenticationBloc,
          ),
          BlocProvider.value(
            value: userProfileBloc,
          ),
        ], child: MaterialApp(home: UserProfilePage())));

        expect(find.byType(Avatar), findsOneWidget);
      });

      testWidgets('email address', (tester) async {
        await tester.pumpWidget(MultiBlocProvider(providers: [
          BlocProvider.value(
            value: authenticationBloc,
          ),
          BlocProvider.value(
            value: userProfileBloc,
          ),
        ], child: MaterialApp(home: UserProfilePage())));

        expect(find.byType(Avatar), findsOneWidget);
      });

      testWidgets('name text form fields', (tester) async {
        await tester.pumpWidget(MultiBlocProvider(providers: [
          BlocProvider.value(
            value: authenticationBloc,
          ),
          BlocProvider.value(
            value: userProfileBloc,
          ),
        ], child: MaterialApp(home: UserProfilePage())));

        expect(find.byType(TextFormField), findsNWidgets(2));
      });

      testWidgets('birthdate and save raised buttons', (tester) async {
        await tester.pumpWidget(MultiBlocProvider(providers: [
          BlocProvider.value(
            value: authenticationBloc,
          ),
          BlocProvider.value(
            value: userProfileBloc,
          ),
        ], child: MaterialApp(home: UserProfilePage())));

        expect(find.byType(RaisedButton), findsNWidgets(2));
      });
    });
  });
}
