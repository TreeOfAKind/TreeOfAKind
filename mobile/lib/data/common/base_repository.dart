import 'package:cqrs/contracts.dart';
import 'package:cqrs/cqrs.dart';

abstract class BaseRepository {
  BaseRepository(this.cqrs) : assert(cqrs != null);

  CQRS cqrs;

  Future<BaseQueryResult<T>> get<T>(IRemoteQuery<T> query) async {
    try {
      final data = await cqrs.get(query);
      print('Query ${query.runtimeType} executed successfully.');
      return BaseQueryResult(data);
    } catch (e) {
      print('Query ${query.runtimeType} failed unexpectedly.');
      return BaseQueryResult<T>(null, error: true);
    }
  }

  Future<BaseCommandResult> run(IRemoteCommand command) async {
    try {
      final result = await cqrs.run(command);

      if (result.success) {
        print('Command ${command.runtimeType} executed successfully.');
        return BaseCommandResult.successful();
      } else {
        final buffer = StringBuffer();
        for (final error in result.errors) {
          buffer.write('${error.message} (${error.code}), ');
        }

        print('Command ${command.runtimeType} failed.'
            ' ValidationErrors: [${buffer.toString()}]');

        final errorCodes = result.errors.map((e) => e.code).toList();
        return BaseCommandResult.validationError(errorCodes);
      }
    } catch (e, s) {
      print('Command ${command.runtimeType} failed unexpectedly.');
      return BaseCommandResult.unexpectedError();
    }
  }
}

class BaseQueryResult<T> {
  BaseQueryResult(this.data, {this.error = false});

  final T data;
  final bool error;
}

class BaseCommandResult {
  BaseCommandResult.unexpectedError()
      : wasSuccessful = false,
        errorCodes = [],
        unexpectedError = true;

  BaseCommandResult.successful()
      : wasSuccessful = true,
        errorCodes = [],
        unexpectedError = false;

  BaseCommandResult.validationError(this.errorCodes)
      : wasSuccessful = false,
        unexpectedError = false;

  final bool wasSuccessful;
  final List<int> errorCodes;
  final bool unexpectedError;
}
