// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'contracts.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

TreeItemDTO _$TreeItemDTOFromJson(Map<String, dynamic> json) {
  return TreeItemDTO(
    id: json['id'] as String,
    photoUri: json['photoUri'] as String,
    treeName: json['treeName'] as String,
  );
}

Map<String, dynamic> _$TreeItemDTOToJson(TreeItemDTO instance) =>
    <String, dynamic>{
      'id': instance.id,
      'photoUri': instance.photoUri,
      'treeName': instance.treeName,
    };

TreesListDTO _$TreesListDTOFromJson(Map<String, dynamic> json) {
  return TreesListDTO(
    trees: (json['trees'] as List)
        ?.map((e) =>
            e == null ? null : TreeItemDTO.fromJson(e as Map<String, dynamic>))
        ?.toList(),
  );
}

Map<String, dynamic> _$TreesListDTOToJson(TreesListDTO instance) =>
    <String, dynamic>{
      'trees': instance.trees,
    };

TreeDTO _$TreeDTOFromJson(Map<String, dynamic> json) {
  return TreeDTO(
    treeId: json['treeId'] as String,
    photoUri: json['photoUri'] as String,
    treeName: json['treeName'] as String,
    people: (json['people'] as List)
        ?.map((e) =>
            e == null ? null : PersonDTO.fromJson(e as Map<String, dynamic>))
        ?.toList(),
  );
}

Map<String, dynamic> _$TreeDTOToJson(TreeDTO instance) => <String, dynamic>{
      'treeId': instance.treeId,
      'photoUri': instance.photoUri,
      'treeName': instance.treeName,
      'people': instance.people,
    };

GetMyTrees _$GetMyTreesFromJson(Map<String, dynamic> json) {
  return GetMyTrees()..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$GetMyTreesToJson(GetMyTrees instance) =>
    <String, dynamic>{
      'endpointRoute': instance.endpointRoute,
    };

GetTree _$GetTreeFromJson(Map<String, dynamic> json) {
  return GetTree(
    treeId: json['treeId'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$GetTreeToJson(GetTree instance) => <String, dynamic>{
      'treeId': instance.treeId,
      'endpointRoute': instance.endpointRoute,
    };

CreateTree _$CreateTreeFromJson(Map<String, dynamic> json) {
  return CreateTree(
    treeName: json['treeName'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$CreateTreeToJson(CreateTree instance) =>
    <String, dynamic>{
      'treeName': instance.treeName,
      'endpointRoute': instance.endpointRoute,
    };

RemoveTreePhoto _$RemoveTreePhotoFromJson(Map<String, dynamic> json) {
  return RemoveTreePhoto(
    treeId: json['treeId'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$RemoveTreePhotoToJson(RemoveTreePhoto instance) =>
    <String, dynamic>{
      'treeId': instance.treeId,
      'endpointRoute': instance.endpointRoute,
    };
