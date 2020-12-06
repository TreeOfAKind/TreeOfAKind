import 'package:json_annotation/json_annotation.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_action.dart';

part 'contracts.g.dart';

@JsonSerializable()
class PingQuery extends Query<PingQueryResponseDTO> {
  final String errorCode;
  final PingQueryResponseDTO pingQueryResponse;

  PingQuery({this.errorCode, this.pingQueryResponse});

  @override
  String get endpointRoute => "ping/pingQuery";

  @override
  PingQueryResponseDTO deserializeResult(json) =>
      PingQueryResponseDTO.fromJson(json);

  @override
  Map<String, dynamic> toJson() => _$PingQueryToJson(this);
}

@JsonSerializable()
class PingQueryResponseDTO {
  final String someString;
  final DateTime someDateTime;
  final double someDouble;

  PingQueryResponseDTO({this.someString, this.someDateTime, this.someDouble});
  factory PingQueryResponseDTO.fromJson(Map<String, dynamic> json) =>
      _$PingQueryResponseDTOFromJson(json);
  Map<String, dynamic> toJson() => _$PingQueryResponseDTOToJson(this);
}
