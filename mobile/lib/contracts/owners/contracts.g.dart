// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'contracts.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

AddTreeOwner _$AddTreeOwnerFromJson(Map<String, dynamic> json) {
  return AddTreeOwner(
    treeId: json['treeId'] as String,
    invitedUserEmail: json['invitedUserEmail'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$AddTreeOwnerToJson(AddTreeOwner instance) =>
    <String, dynamic>{
      'treeId': instance.treeId,
      'invitedUserEmail': instance.invitedUserEmail,
      'endpointRoute': instance.endpointRoute,
    };

RemoveTreeOwner _$RemoveTreeOwnerFromJson(Map<String, dynamic> json) {
  return RemoveTreeOwner(
    treeId: json['treeId'] as String,
    removedUserId: json['removedUserId'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$RemoveTreeOwnerToJson(RemoveTreeOwner instance) =>
    <String, dynamic>{
      'treeId': instance.treeId,
      'removedUserId': instance.removedUserId,
      'endpointRoute': instance.endpointRoute,
    };
