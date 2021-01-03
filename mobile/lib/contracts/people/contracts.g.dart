// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'contracts.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

FileDTO _$FileDTOFromJson(Map<String, dynamic> json) {
  return FileDTO(
    id: json['id'] as String,
    name: json['name'] as String,
    contentType: json['contentType'] as String,
    uri: json['uri'] as String,
  );
}

Map<String, dynamic> _$FileDTOToJson(FileDTO instance) => <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'contentType': instance.contentType,
      'uri': instance.uri,
    };

PersonDTO _$PersonDTOFromJson(Map<String, dynamic> json) {
  return PersonDTO(
    id: json['id'] as String,
    name: json['name'] as String,
    lastName: json['lastName'] as String,
    gender: json['gender'] as String,
    birthDate: json['birthDate'] == null
        ? null
        : DateTime.parse(json['birthDate'] as String),
    deathDate: json['deathDate'] == null
        ? null
        : DateTime.parse(json['deathDate'] as String),
    description: json['description'] as String,
    biography: json['biography'] as String,
    mother: json['mother'] as String,
    father: json['father'] as String,
    spouse: json['spouse'] as String,
    children: (json['children'] as List)?.map((e) => e as String)?.toList(),
    unknownRelations:
        (json['unknownRelations'] as List)?.map((e) => e as String)?.toList(),
    mainPhoto: json['mainPhoto'] == null
        ? null
        : FileDTO.fromJson(json['mainPhoto'] as Map<String, dynamic>),
    files: (json['files'] as List)
        ?.map((e) =>
            e == null ? null : FileDTO.fromJson(e as Map<String, dynamic>))
        ?.toList(),
  );
}

Map<String, dynamic> _$PersonDTOToJson(PersonDTO instance) => <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'lastName': instance.lastName,
      'gender': instance.gender,
      'birthDate': instance.birthDate?.toIso8601String(),
      'deathDate': instance.deathDate?.toIso8601String(),
      'description': instance.description,
      'biography': instance.biography,
      'mother': instance.mother,
      'father': instance.father,
      'spouse': instance.spouse,
      'children': instance.children,
      'unknownRelations': instance.unknownRelations,
      'mainPhoto': instance.mainPhoto,
      'files': instance.files,
    };

AddPerson _$AddPersonFromJson(Map<String, dynamic> json) {
  return AddPerson(
    treeId: json['treeId'] as String,
    name: json['name'] as String,
    lastName: json['lastName'] as String,
    gender: json['gender'] as String,
    birthDate: json['birthDate'] == null
        ? null
        : DateTime.parse(json['birthDate'] as String),
    deathDate: json['deathDate'] == null
        ? null
        : DateTime.parse(json['deathDate'] as String),
    description: json['description'] as String,
    biography: json['biography'] as String,
    mother: json['mother'] as String,
    father: json['father'] as String,
    spouse: json['spouse'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$AddPersonToJson(AddPerson instance) => <String, dynamic>{
      'treeId': instance.treeId,
      'name': instance.name,
      'lastName': instance.lastName,
      'gender': instance.gender,
      'birthDate': instance.birthDate?.toIso8601String(),
      'deathDate': instance.deathDate?.toIso8601String(),
      'description': instance.description,
      'biography': instance.biography,
      'mother': instance.mother,
      'father': instance.father,
      'spouse': instance.spouse,
      'endpointRoute': instance.endpointRoute,
    };

UpdatePerson _$UpdatePersonFromJson(Map<String, dynamic> json) {
  return UpdatePerson(
    treeId: json['treeId'] as String,
    personId: json['personId'] as String,
    name: json['name'] as String,
    lastName: json['lastName'] as String,
    gender: json['gender'] as String,
    birthDate: json['birthDate'] == null
        ? null
        : DateTime.parse(json['birthDate'] as String),
    deathDate: json['deathDate'] == null
        ? null
        : DateTime.parse(json['deathDate'] as String),
    description: json['description'] as String,
    biography: json['biography'] as String,
    mother: json['mother'] as String,
    father: json['father'] as String,
    spouse: json['spouse'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$UpdatePersonToJson(UpdatePerson instance) =>
    <String, dynamic>{
      'treeId': instance.treeId,
      'personId': instance.personId,
      'name': instance.name,
      'lastName': instance.lastName,
      'gender': instance.gender,
      'birthDate': instance.birthDate?.toIso8601String(),
      'deathDate': instance.deathDate?.toIso8601String(),
      'description': instance.description,
      'biography': instance.biography,
      'mother': instance.mother,
      'father': instance.father,
      'spouse': instance.spouse,
      'endpointRoute': instance.endpointRoute,
    };

RemovePerson _$RemovePersonFromJson(Map<String, dynamic> json) {
  return RemovePerson(
    treeId: json['treeId'] as String,
    personId: json['personId'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$RemovePersonToJson(RemovePerson instance) =>
    <String, dynamic>{
      'treeId': instance.treeId,
      'personId': instance.personId,
      'endpointRoute': instance.endpointRoute,
    };
