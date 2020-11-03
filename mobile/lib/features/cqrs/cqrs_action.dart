abstract class CqrsAction {
  Map<String, dynamic> toJson();
  String endpointRoute;
}

abstract class Query<T> extends CqrsAction {
  T deserializeResult(dynamic json);
}

abstract class Command implements CqrsAction {}
