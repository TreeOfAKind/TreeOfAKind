import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/resources/app_colors.dart';

final theme = ThemeData(
  primaryColorDark: const Color(0xFF0097A7),
  primaryColorLight: const Color(0xFFB2EBF2),
  primaryColor: AppColors.main,
  accentColor: const Color(0xFF009688),
  scaffoldBackgroundColor: const Color(0xFFE0F2F1),
  inputDecorationTheme: InputDecorationTheme(
    border: OutlineInputBorder(
      borderRadius: BorderRadius.circular(8),
    ),
  ),
  visualDensity: VisualDensity.adaptivePlatformDensity,
);
