abstract class CommandResult {}

class SuccessCommandResult extends CommandResult {
  SuccessCommandResult({this.entityId});

  SuccessCommandResult.fromJson(Map<String, dynamic> json)
      : entityId = json['Id'] as String;

  final String entityId;
}

class FailureCommandResult extends CommandResult {
  FailureCommandResult.fromJson(Map<String, dynamic> json);
}
