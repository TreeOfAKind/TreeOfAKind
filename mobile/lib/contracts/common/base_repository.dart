import 'package:tree_of_a_kind/features/cqrs/command_result.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_action.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_client.dart';

abstract class BaseRepository {
  BaseRepository(this.cqrs) : assert(cqrs != null);

  CqrsClient cqrs;

  Future<BaseQueryResult<T>> get<T>(Query<T> query) async {
    try {
      final data = await cqrs.get(query);
      print('Query ${query.runtimeType} executed successfully.');
      return BaseQueryResult(data);
    } catch (e) {
      print('Query ${query.runtimeType} failed unexpectedly.');
      return BaseQueryResult<T>(null, unexpectedError: true);
    }
  }

  Future<BaseCommandResult> run(Command command) async {
    try {
      final result = await cqrs.run(command);

      if (result is SuccessCommandResult) {
        print('Command ${command.runtimeType} executed successfully.');
        return BaseCommandResult.successful(result);
      } else {
        print('Command ${command.runtimeType} did not pass validation.');
        return BaseCommandResult.validationError(result);
      }
    } catch (e) {
      print('Command ${command.runtimeType} failed unexpectedly.');
      return BaseCommandResult.unexpectedError();
    }
  }

  Future<BaseCommandResult> runWithFile(CommandWithFile command) async {
    try {
      final result = await cqrs.runWithFile(command);

      if (result is SuccessCommandResult) {
        print('Command ${command.runtimeType} executed successfully.');
        return BaseCommandResult.successful(result);
      } else {
        print('Command ${command.runtimeType} did not pass validation.');
        return BaseCommandResult.validationError(result);
      }
    } catch (e) {
      print('Command ${command.runtimeType} failed unexpectedly.');
      return BaseCommandResult.unexpectedError();
    }
  }
}

class BaseQueryResult<T> {
  BaseQueryResult(this.data, {this.unexpectedError = false});

  final T data;
  final bool unexpectedError;
}

class BaseCommandResult {
  static const String genericErrorText =
      'The servers have failed unexpectedly 😔';

  BaseCommandResult.unexpectedError()
      : wasSuccessful = false,
        entityId = null,
        unexpectedError = true,
        errorText = genericErrorText;

  BaseCommandResult.validationError(FailureCommandResult commandResult)
      : wasSuccessful = false,
        entityId = null,
        unexpectedError = false,
        errorText = commandResult.details ?? genericErrorText;

  BaseCommandResult.successful(SuccessCommandResult commandResult)
      : wasSuccessful = true,
        entityId = commandResult.entityId,
        unexpectedError = false,
        errorText = null;

  final bool wasSuccessful;
  final String entityId;
  final bool unexpectedError;
  final String errorText;
}
