import 'package:flutter/material.dart';

import 'app/app.dart';
import 'app/config.dart';
import 'app/dependencies_factory.dart';
import 'app/dependencies_injector.dart';

Future<void> mainCommon(Config config) async {
  WidgetsFlutterBinding.ensureInitialized();
  final dependenciesFactory = AppDependenciesFactory();
  await dependenciesFactory.initializeAsync(config);
  runApp(DependenciesInjector(dependenciesFactory, config, App()));
}
