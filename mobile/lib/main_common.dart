import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/app/dependencies_factory.dart';
import 'package:tree_of_a_kind/app/dependencies_injector.dart';

import 'app/app.dart';
import 'app/config.dart';

Future<void> mainCommon(Config config) async {
  WidgetsFlutterBinding.ensureInitialized();
  final dependenciesFactory = AppDependenciesFactory();
  await dependenciesFactory.initializeAsync(config);
  runApp(DependenciesInjector(dependenciesFactory, config, App()));
}
