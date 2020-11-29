import 'package:flutter/material.dart';

import 'app/app.dart';
import 'app/config.dart';
import 'app/dependencies_factory.dart';
import 'app/dependencies_injector.dart';
import 'contracts/ping/contracts.dart';

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
