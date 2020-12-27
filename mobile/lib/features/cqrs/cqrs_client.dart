import 'dart:convert';
import 'dart:io';

import 'package:dio/dio.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:http/http.dart' as http;
import 'package:tree_of_a_kind/features/cqrs/command_result.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_action.dart';

class CqrsClient {
  CqrsClient(this._apiUri, {FirebaseAuth firebaseAuth})
      : assert(_apiUri != null),
        _firebaseAuth = firebaseAuth ?? FirebaseAuth.instance;

  final Uri _apiUri;
  final FirebaseAuth _firebaseAuth;
  final Duration timeout = const Duration(seconds: 30);

  Future<T> get<T>(Query<T> query) async {
    assert(query != null);

    final response = await _send(query);

    if (response.statusCode >= 200 && response.statusCode < 300) {
      return query.deserializeResult(jsonDecode(response.body));
    } else {
      throw HttpException(
          'Could not fetch query with code ${response.statusCode}',
          uri: response.request.url);
    }
  }

  Future<CommandResult> run<T>(Command command) async {
    assert(command != null);

    final response = await _send(command);

    if (response.statusCode >= 200 && response.statusCode < 300) {
      return response.body.isEmpty
          ? SuccessCommandResult()
          : SuccessCommandResult.fromJson(jsonDecode(response.body));
    } else if (response.statusCode == 422) {
      return response.body.isEmpty
          ? FailureCommandResult()
          : FailureCommandResult.fromJson(jsonDecode(response.body));
    } else {
      throw HttpException(
          'Could not run command with code ${response.statusCode}',
          uri: response.request.url);
    }
  }

  Future<CommandResult> runWithFile<T>(CommandWithFile command) async {
    assert(command != null);

    final formData = new FormData.fromMap(command.data);
    final token = await _firebaseAuth.currentUser.getIdToken();

    final response = await Dio(BaseOptions(headers: {
      HttpHeaders.contentTypeHeader: 'multipart/form-data',
      HttpHeaders.authorizationHeader: "Bearer $token"
    })).post(_apiUri.resolve(command.endpointRoute).toString(), data: formData);

    if (response.statusCode >= 200 && response.statusCode < 300) {
      return SuccessCommandResult.fromJson(response.data);
    } else if (response.statusCode == 422) {
      return FailureCommandResult.fromJson(response.data);
    } else {
      throw HttpException(
          'Could not run command with code ${response.statusCode}',
          uri: response.request.uri);
    }
  }

  Future<Map<String, dynamic>> runSpecial<T>(Command command) async {
    assert(command != null);

    final response = await _send(command);

    if (response.statusCode >= 200 && response.statusCode < 300) {
      return jsonDecode(response.body);
    } else {
      throw HttpException(
          'Could not run command with code ${response.statusCode}',
          uri: response.request.url);
    }
  }

  Future<http.Response> _send(JsonCqrsAction cqrsAction) async {
    assert(cqrsAction != null);

    final token = await _firebaseAuth.currentUser.getIdToken();

    Map<String, String> headers = {
      HttpHeaders.contentTypeHeader: 'application/json'
    };

    if (token != null) {
      headers[HttpHeaders.authorizationHeader] = "Bearer $token";
    }

    return http.post(_apiUri.resolve(cqrsAction.endpointRoute),
        body: jsonEncode(cqrsAction), headers: headers);
  }
}
