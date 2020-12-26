abstract class CqrsAction {
  String endpointRoute;
}

abstract class JsonCqrsAction extends CqrsAction {
  Map<String, dynamic> toJson();
}

abstract class Query<T> extends JsonCqrsAction {
  T deserializeResult(dynamic json);
}

abstract class Command extends JsonCqrsAction {}

abstract class CommandWithFile extends CqrsAction {
  Map<String, dynamic> data;
}
