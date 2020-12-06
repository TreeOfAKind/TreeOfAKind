// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'contracts.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserProfileDTO _$UserProfileDTOFromJson(Map<String, dynamic> json) {
  return UserProfileDTO(
    id: json['id'] as String,
    firstName: json['firstName'] as String,
    lastName: json['lastName'] as String,
    birthDate: json['birthDate'] == null
        ? null
        : DateTime.parse(json['birthDate'] as String),
  );
}

Map<String, dynamic> _$UserProfileDTOToJson(UserProfileDTO instance) =>
    <String, dynamic>{
      'id': instance.id,
      'firstName': instance.firstName,
      'lastName': instance.lastName,
      'birthDate': instance.birthDate?.toIso8601String(),
    };

GetMyUserProfile _$GetMyUserProfileFromJson(Map<String, dynamic> json) {
  return GetMyUserProfile()..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$GetMyUserProfileToJson(GetMyUserProfile instance) =>
    <String, dynamic>{
      'endpointRoute': instance.endpointRoute,
    };

CreateOrUpdateUserProfile _$CreateOrUpdateUserProfileFromJson(
    Map<String, dynamic> json) {
  return CreateOrUpdateUserProfile(
    firstName: json['firstName'] as String,
    lastName: json['lastName'] as String,
    birthDate: json['birthDate'] == null
        ? null
        : DateTime.parse(json['birthDate'] as String),
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$CreateOrUpdateUserProfileToJson(
        CreateOrUpdateUserProfile instance) =>
    <String, dynamic>{
      'firstName': instance.firstName,
      'lastName': instance.lastName,
      'birthDate': instance.birthDate?.toIso8601String(),
      'endpointRoute': instance.endpointRoute,
    };
