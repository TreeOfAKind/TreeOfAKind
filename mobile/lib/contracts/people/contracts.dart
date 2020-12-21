import 'package:json_annotation/json_annotation.dart';

part 'contracts.g.dart';

@JsonSerializable()
class PersonDTO {
  final String id;
  final String name;
  final String lastName;
  final String gender;
  final DateTime birthDate;
  final DateTime deathDate;
  final String description;
  final String biography;
  final String mother;
  final String father;
  final List<String> spouses;
  final List<String> children;
  final List<String> unknownRelations;

  PersonDTO(
      {this.id,
      this.name,
      this.lastName,
      this.gender,
      this.birthDate,
      this.deathDate,
      this.description,
      this.biography,
      this.mother,
      this.father,
      this.spouses,
      this.children,
      this.unknownRelations});

  factory PersonDTO.fromJson(Map<String, dynamic> json) =>
      _$PersonDTOFromJson(json);

  Map<String, dynamic> toJson() => _$PersonDTOToJson(this);
}
