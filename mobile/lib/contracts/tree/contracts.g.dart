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
    owners: (json['owners'] as List)
        ?.map((e) => e == null
            ? null
            : UserProfileDTO.fromJson(e as Map<String, dynamic>))
        ?.toList(),
  );
}

Map<String, dynamic> _$TreeDTOToJson(TreeDTO instance) => <String, dynamic>{
      'treeId': instance.treeId,
      'photoUri': instance.photoUri,
      'treeName': instance.treeName,
      'people': instance.people,
      'owners': instance.owners,
    };

TreeStatsDTO _$TreeStatsDTOFromJson(Map<String, dynamic> json) {
  return TreeStatsDTO(
    totalNumberOfPeople: json['totalNumberOfPeople'] as int,
    averageLifespanInDays: (json['averageLifespanInDays'] as num)?.toDouble(),
    numberOfPeopleOfEachGender:
        (json['numberOfPeopleOfEachGender'] as Map<String, dynamic>)?.map(
      (k, e) => MapEntry(k, e as int),
    ),
    numberOfLivingPeople: json['numberOfLivingPeople'] as int,
    numberOfDeceasedPeople: json['numberOfDeceasedPeople'] as int,
    averageNumberOfChildren:
        (json['averageNumberOfChildren'] as num)?.toDouble(),
    numberOfMarriedPeople: json['numberOfMarriedPeople'] as int,
    numberOfSinglePeople: json['numberOfSinglePeople'] as int,
  );
}

Map<String, dynamic> _$TreeStatsDTOToJson(TreeStatsDTO instance) =>
    <String, dynamic>{
      'totalNumberOfPeople': instance.totalNumberOfPeople,
      'averageLifespanInDays': instance.averageLifespanInDays,
      'numberOfPeopleOfEachGender': instance.numberOfPeopleOfEachGender,
      'numberOfLivingPeople': instance.numberOfLivingPeople,
      'numberOfDeceasedPeople': instance.numberOfDeceasedPeople,
      'averageNumberOfChildren': instance.averageNumberOfChildren,
      'numberOfMarriedPeople': instance.numberOfMarriedPeople,
      'numberOfSinglePeople': instance.numberOfSinglePeople,
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

GetTreeStatistics _$GetTreeStatisticsFromJson(Map<String, dynamic> json) {
  return GetTreeStatistics(
    treeId: json['treeId'] as String,
  )..endpointRoute = json['endpointRoute'] as String;
}

Map<String, dynamic> _$GetTreeStatisticsToJson(GetTreeStatistics instance) =>
    <String, dynamic>{
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
