// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'contracts.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

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
    spouses: (json['spouses'] as List)?.map((e) => e as String)?.toList(),
    unknownRelations:
        (json['unknownRelations'] as List)?.map((e) => e as String)?.toList(),
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
      'spouses': instance.spouses,
      'unknownRelations': instance.unknownRelations,
    };
