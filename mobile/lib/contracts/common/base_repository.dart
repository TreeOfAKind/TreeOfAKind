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

      if (result.runtimeType == SuccessCommandResult) {
        print('Command ${command.runtimeType} executed successfully.');
        var success = result as SuccessCommandResult;
        return BaseCommandResult.successful(success.entityId);
      } else {
        // TODO: Add validation handling
        print('Command ${command.runtimeType} failed.');
        return BaseCommandResult.unexpectedError();
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
  BaseCommandResult.unexpectedError()
      : wasSuccessful = false,
        entityId = null,
        unexpectedError = true;

  BaseCommandResult.successful(String entityId)
      : wasSuccessful = true,
        entityId = entityId,
        unexpectedError = false;

  final bool wasSuccessful;
  final String entityId;
  final bool unexpectedError;
}
