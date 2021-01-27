import 'package:json_annotation/json_annotation.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_action.dart';

part 'contracts.g.dart';

@JsonSerializable()
class AddTreeOwner extends Command {
  String treeId;
  String invitedUserEmail;

  @override
  String get endpointRoute => "Tree/AddTreeOwner";

  AddTreeOwner({this.treeId, this.invitedUserEmail});

  Map<String, dynamic> toJson() => _$AddTreeOwnerToJson(this);
}

@JsonSerializable()
class RemoveTreeOwner extends Command {
  String treeId;
  String removedUserId;

  @override
  String get endpointRoute => "Tree/RemoveTreeOwner";

  RemoveTreeOwner({this.treeId, this.removedUserId});

  Map<String, dynamic> toJson() => _$RemoveTreeOwnerToJson(this);
}
