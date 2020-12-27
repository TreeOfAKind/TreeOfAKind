import 'package:json_annotation/json_annotation.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_action.dart';

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

@JsonSerializable()
class AddPerson extends Command {
  final String treeId;
  final String name;
  final String lastName;
  final String gender;
  final DateTime birthDate;
  final DateTime deathDate;
  final String description;
  final String biography;
  final String mother;
  final String father;
  final String spouse;

  @override
  String get endpointRoute => "People/AddPerson";

  AddPerson(
      {this.treeId,
      this.name,
      this.lastName,
      this.gender,
      this.birthDate,
      this.deathDate,
      this.description,
      this.biography,
      this.mother,
      this.father,
      this.spouse});

  Map<String, dynamic> toJson() => _$AddPersonToJson(this);
}

@JsonSerializable()
class RemovePerson extends Command {
  String treeId;
  String personId;

  @override
  String get endpointRoute => "People/RemovePerson";

  RemovePerson({this.treeId, this.personId});

  Map<String, dynamic> toJson() => _$RemovePersonToJson(this);
}
