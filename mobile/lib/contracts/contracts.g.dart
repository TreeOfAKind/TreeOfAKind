// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'contracts.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

PingQuery _$PingQueryFromJson(Map<String, dynamic> json) {
  return PingQuery(
    errorCode: json['errorCode'] as String,
    pingQueryResponse: json['pingQueryResponse'] == null
        ? null
        : PingQueryResponseDTO.fromJson(
            json['pingQueryResponse'] as Map<String, dynamic>),
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$PingQueryToJson(PingQuery instance) => <String, dynamic>{
      'errorCode': instance.errorCode,
      'pingQueryResponse': instance.pingQueryResponse,
      'endpointRoute': instance.endpointRoute,
    };

PingQueryResponseDTO _$PingQueryResponseDTOFromJson(Map<String, dynamic> json) {
  return PingQueryResponseDTO(
    someString: json['someString'] as String,
    someDateTime: json['someDateTime'] == null
        ? null
        : DateTime.parse(json['someDateTime'] as String),
    someDouble: (json['someDouble'] as num)?.toDouble(),
  );
}

Map<String, dynamic> _$PingQueryResponseDTOToJson(
        PingQueryResponseDTO instance) =>
    <String, dynamic>{
      'someString': instance.someString,
      'someDateTime': instance.someDateTime?.toIso8601String(),
      'someDouble': instance.someDouble,
    };
