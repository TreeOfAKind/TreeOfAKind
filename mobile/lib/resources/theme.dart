import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/resources/app_colors.dart';

final theme = ThemeData(
  primaryColorDark: Colors.green.shade600,
  primaryColorLight: Colors.green.shade200,
  primaryColor: Colors.green,
  accentColor: Colors.lime,
  scaffoldBackgroundColor: const Color(0xFFE0F2F1),
  inputDecorationTheme: InputDecorationTheme(
    border: OutlineInputBorder(
      borderRadius: BorderRadius.circular(8),
    ),
  ),
  visualDensity: VisualDensity.adaptivePlatformDensity,
);
