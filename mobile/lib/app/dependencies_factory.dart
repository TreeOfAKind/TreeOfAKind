import 'package:authentication_repository/authentication_repository.dart';
import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:flutter/foundation.dart';
import 'package:tree_of_a_kind/app/config.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/contracts/user_profile/user_profile_repository.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_client.dart';

import 'simple_bloc_observer.dart';

abstract class DependenciesFactory {
  Future<void> initializeAsync(Config config);

  CqrsClient cqrsClient(Uri apiUri, {FirebaseAuth firebaseAuth});

  AuthenticationRepository authenticationRepository();
  UserProfileRepository userProfileRepository(CqrsClient cqrs);
  TreeRepository treeRepository(CqrsClient cqrs);
  PeopleRepository peopleRepository(CqrsClient cqrs);
}

class AppDependenciesFactory extends DependenciesFactory {
  @override
  AuthenticationRepository authenticationRepository(
      {FirebaseAuth firebaseAuth}) {
    return AuthenticationRepository(firebaseAuth: firebaseAuth);
  }

  @override
  UserProfileRepository userProfileRepository(CqrsClient cqrs) {
    return UserProfileRepository(cqrs);
  }

  @override
  TreeRepository treeRepository(CqrsClient cqrs) {
    return TreeRepository(cqrs);
  }

  @override
  PeopleRepository peopleRepository(CqrsClient cqrs) {
    return PeopleRepository(cqrs);
  }

  @override
  CqrsClient cqrsClient(Uri apiUri, {FirebaseAuth firebaseAuth}) {
    return CqrsClient(apiUri, firebaseAuth: firebaseAuth);
  }

  @override
  Future<void> initializeAsync(Config config) async {
    await Firebase.initializeApp();
    EquatableConfig.stringify = kDebugMode;
    Bloc.observer = SimpleBlocObserver();
  }
}
