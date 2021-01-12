import 'package:dio/dio.dart';
import 'package:flutter/foundation.dart';
import 'package:json_annotation/json_annotation.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_action.dart';

part 'contracts.g.dart';

@JsonSerializable()
class TreeItemDTO {
  final String id;
  final String photoUri;
  final String treeName;

  TreeItemDTO({this.id, this.photoUri, this.treeName});

  factory TreeItemDTO.fromJson(Map<String, dynamic> json) =>
      _$TreeItemDTOFromJson(json);

  Map<String, dynamic> toJson() => _$TreeItemDTOToJson(this);
}

@JsonSerializable()
class TreesListDTO {
  final List<TreeItemDTO> trees;

  TreesListDTO({this.trees});

  factory TreesListDTO.fromJson(Map<String, dynamic> json) =>
      _$TreesListDTOFromJson(json);

  Map<String, dynamic> toJson() => _$TreesListDTOToJson(this);
}

@JsonSerializable()
class TreeDTO {
  String treeId;
  String photoUri;
  String treeName;
  List<PersonDTO> people;
  List<UserProfileDTO> owners;

  TreeDTO(
      {this.treeId, this.photoUri, this.treeName, this.people, this.owners});

  factory TreeDTO.fromJson(Map<String, dynamic> json) =>
      _$TreeDTOFromJson(json);

  Map<String, dynamic> toJson() => _$TreeDTOToJson(this);
}

@JsonSerializable()
class TreeStatsDTO {
  final int totalNumberOfPeople;
  final double averageLifespanInDays;
  final Map<String, int> numberOfPeopleOfEachGender;
  final int numberOfLivingPeople;
  final int numberOfDeceasedPeople;
  final double averageNumberOfChildren;
  final int numberOfMarriedPeople;
  final int numberOfSinglePeople;

  TreeStatsDTO(
      {this.totalNumberOfPeople,
      this.averageLifespanInDays,
      this.numberOfPeopleOfEachGender,
      this.numberOfLivingPeople,
      this.numberOfDeceasedPeople,
      this.averageNumberOfChildren,
      this.numberOfMarriedPeople,
      this.numberOfSinglePeople});

  factory TreeStatsDTO.fromJson(Map<String, dynamic> json) =>
      _$TreeStatsDTOFromJson(json);

  Map<String, dynamic> toJson() => _$TreeStatsDTOToJson(this);
}

@JsonSerializable()
class GetMyTrees extends Query<TreesListDTO> {
  @override
  String get endpointRoute => "Tree/GetMyTrees";

  @override
  TreesListDTO deserializeResult(json) => TreesListDTO.fromJson(json);

  @override
  Map<String, dynamic> toJson() => _$GetMyTreesToJson(this);
}

@JsonSerializable()
class GetTree extends Query<TreeDTO> {
  String treeId;

  @override
  String get endpointRoute => "Tree/GetTree";

  GetTree({this.treeId});

  @override
  TreeDTO deserializeResult(json) => TreeDTO.fromJson(json);

  @override
  Map<String, dynamic> toJson() => _$GetTreeToJson(this);
}

@JsonSerializable()
class GetTreeStatistics extends Query<TreeStatsDTO> {
  String treeId;

  @override
  String get endpointRoute => "Tree/GetTreeStatistics";

  GetTreeStatistics({this.treeId});

  @override
  TreeStatsDTO deserializeResult(json) => TreeStatsDTO.fromJson(json);

  @override
  Map<String, dynamic> toJson() => _$GetTreeStatisticsToJson(this);
}

@JsonSerializable()
class CreateTree extends Command {
  String treeName;

  @override
  String get endpointRoute => "Tree/CreateTree";

  CreateTree({this.treeName});

  Map<String, dynamic> toJson() => _$CreateTreeToJson(this);
}

@JsonSerializable()
class UpdateTreeName extends Command {
  String treeId;
  String treeName;

  @override
  String get endpointRoute => "Tree/UpdateTreeName";

  UpdateTreeName({this.treeId, this.treeName});

  Map<String, dynamic> toJson() => _$UpdateTreeNameToJson(this);
}

class AddOrChangeTreePhoto extends CommandWithFile {
  AddOrChangeTreePhoto(
      {@required String treeId, @required MultipartFile image}) {
    data['TreeId'] = treeId;
    data['Image'] = image;
  }

  @override
  String get endpointRoute => "Tree/AddOrChangeTreePhoto";
}

@JsonSerializable()
class RemoveTreePhoto extends Command {
  String treeId;

  @override
  String get endpointRoute => "Tree/RemoveTreePhoto";

  RemoveTreePhoto({this.treeId});

  Map<String, dynamic> toJson() => _$RemoveTreePhotoToJson(this);
}

@JsonSerializable()
class MergeTrees extends Command {
  String firstTreeId;
  String secondTreeId;

  @override
  String get endpointRoute => "Tree/MergeTrees";

  MergeTrees({this.firstTreeId, this.secondTreeId});

  Map<String, dynamic> toJson() => _$MergeTreesToJson(this);
}
