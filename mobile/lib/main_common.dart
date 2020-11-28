import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/app/dependencies_factory.dart';
import 'package:tree_of_a_kind/app/dependencies_injector.dart';
import 'package:tree_of_a_kind/contracts/contracts.dart';

import 'app/app.dart';
import 'app/config.dart';

Future<void> mainCommon(Config config) async {
  WidgetsFlutterBinding.ensureInitialized();
  final dependenciesFactory = AppDependenciesFactory();
  await dependenciesFactory.initializeAsync(config);
  final cqrs = dependenciesFactory.cqrsClient(config.apiUri);
  final ping = await cqrs.get(PingQuery(
      pingQueryResponse: PingQueryResponseDTO(
          someString: "A string",
          someDateTime: DateTime.now(),
          someDouble: 0))); // TODO: Remove once works
  print(ping.someString);
  runApp(DependenciesInjector(dependenciesFactory, config, App()));
}
