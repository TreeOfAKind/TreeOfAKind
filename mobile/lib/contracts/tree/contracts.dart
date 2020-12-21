import 'package:json_annotation/json_annotation.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
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
  final String treeId;
  final String photoUri;
  final String treeName;
  final List<PersonDTO> people;

  TreeDTO({this.treeId, this.photoUri, this.treeName, this.people});

  factory TreeDTO.fromJson(Map<String, dynamic> json) =>
      _$TreeDTOFromJson(json);

  Map<String, dynamic> toJson() => _$TreeDTOToJson(this);
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
class CreateTree extends Command {
  String treeName;

  @override
  String get endpointRoute => "Tree/CreateTree";

  CreateTree({this.treeName});

  Map<String, dynamic> toJson() => _$CreateTreeToJson(this);
}
