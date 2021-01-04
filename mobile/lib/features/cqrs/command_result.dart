abstract class CommandResult {}

class SuccessCommandResult extends CommandResult {
  SuccessCommandResult({this.entityId});

  SuccessCommandResult.fromJson(Map<String, dynamic> json)
      : entityId = json['id'] as String;

  final String entityId;
}

class FailureCommandResult extends CommandResult {
  FailureCommandResult({this.errorCode, this.title, this.details});

  final String errorCode;
  final String title;
  final String details;

  FailureCommandResult.fromJson(Map<String, dynamic> json)
      : errorCode = json['errorCode'] as String,
        title = json['title'] as String,
        details = json['detail'] as String;
}
