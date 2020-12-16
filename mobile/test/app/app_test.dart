import 'package:authentication_repository/authentication_repository.dart';
import 'package:bloc_test/bloc_test.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/app/app.dart';
import 'package:tree_of_a_kind/contracts/common/base_repository.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:tree_of_a_kind/features/common/splash_page.dart';
import 'package:tree_of_a_kind/features/home/bloc/tree_list_bloc.dart';
import 'package:tree_of_a_kind/features/home/home.dart';
import 'package:tree_of_a_kind/features/login/login.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mockito/mockito.dart';

// ignore: must_be_immutable
class MockUser extends Mock implements User {
  @override
  String get email => 'joe@gmail.com';
}

class MockAuthenticationRepository extends Mock
    implements AuthenticationRepository {}

class MockTreeRepository extends Mock implements TreeRepository {}

class MockAuthenticationBloc extends MockBloc<AuthenticationState>
    implements AuthenticationBloc {}

class MockTreeListBloc extends MockBloc<TreeListState> implements TreeListBloc {
}

void main() {
  group('App', () {
    AuthenticationBloc authenticationBloc;
    AuthenticationRepository authenticationRepository;
    TreeRepository treeRepository;

    setUp(() {
      authenticationBloc = MockAuthenticationBloc();
      authenticationRepository = MockAuthenticationRepository();
      treeRepository = MockTreeRepository();

      final treeItem = TreeItemDTO(id: "id_1", treeName: "Test tree");
      when(treeRepository.getMyTrees())
          .thenAnswer((inv) => Future.value(BaseQueryResult([treeItem])));
    });

    testWidgets('renders SplashPage by default', (tester) async {
      await tester.pumpWidget(
        BlocProvider.value(value: authenticationBloc, child: App()),
      );
      await tester.pumpAndSettle();
      expect(find.byType(SplashPage), findsOneWidget);
    });

    testWidgets('navigates to LoginPage when status is unauthenticated',
        (tester) async {
      whenListen(
        authenticationBloc,
        Stream.value(const AuthenticationState.unauthenticated()),
      );
      await tester.pumpWidget(
        RepositoryProvider.value(
          value: authenticationRepository,
          child: BlocProvider.value(
            value: authenticationBloc,
            child: App(),
          ),
        ),
      );
      await tester.pumpAndSettle();
      expect(find.byType(LoginPage), findsOneWidget);
    });

    testWidgets('navigates to HomePage when status is authenticated',
        (tester) async {
      whenListen(
        authenticationBloc,
        Stream.value(AuthenticationState.authenticated(MockUser())),
      );

      await tester.pumpWidget(MultiRepositoryProvider(
        providers: [
          RepositoryProvider.value(
            value: authenticationRepository,
          ),
          RepositoryProvider.value(
            value: treeRepository,
          )
        ],
        child: BlocProvider.value(
          value: authenticationBloc,
          child: App(),
        ),
      ));

      await tester.pumpAndSettle();
      expect(find.byType(HomePage), findsOneWidget);
    });
  });
}
