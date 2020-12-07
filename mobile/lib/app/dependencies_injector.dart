import 'package:authentication_repository/authentication_repository.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:provider/provider.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/contracts/user_profile/user_profile_repository.dart';
import 'package:tree_of_a_kind/features/authentication/bloc/authentication_bloc.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_client.dart';
import 'package:tree_of_a_kind/features/user_profile/bloc/user_profile_bloc.dart';

import 'config.dart';
import 'dependencies_factory.dart';

class DependenciesInjector extends StatelessWidget {
  const DependenciesInjector(
      this._dependenciesFactory, this._config, this._child)
      : assert(_dependenciesFactory != null),
        assert(_config != null);

  final DependenciesFactory _dependenciesFactory;
  final Config _config;
  final Widget _child;

  @override
  Widget build(BuildContext context) {
    final cqrsClient = _dependenciesFactory.cqrsClient(_config.apiUri);

    final authenticationRepository =
        _dependenciesFactory.authenticationRepository();

    return MultiProvider(
        providers: [
          Provider<Config>.value(value: _config),
          Provider<CqrsClient>.value(value: cqrsClient),
        ],
        child: MultiRepositoryProvider(
          providers: [
            RepositoryProvider<AuthenticationRepository>.value(
                value: authenticationRepository),
            RepositoryProvider<UserProfileRepository>.value(
                value: _dependenciesFactory.userProfileRepository(cqrsClient)),
            RepositoryProvider<TreeRepository>.value(
                value: _dependenciesFactory.treeRepository(cqrsClient))
          ],
          child: MultiBlocProvider(
            providers: [
              BlocProvider<AuthenticationBloc>.value(
                value: AuthenticationBloc(
                    authenticationRepository: authenticationRepository),
              ),
            ],
            child: _child,
          ),
        ));
  }
}
