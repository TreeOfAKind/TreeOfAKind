import 'package:json_annotation/json_annotation.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_action.dart';

part 'contracts.g.dart';

@JsonSerializable()
class UserProfileDTO {
  final String id;
  final String firstName;
  final String lastName;
  final DateTime birthDate;

  UserProfileDTO({this.id, this.firstName, this.lastName, this.birthDate});
  factory UserProfileDTO.fromJson(Map<String, dynamic> json) =>
      _$UserProfileDTOFromJson(json);
  Map<String, dynamic> toJson() => _$UserProfileDTOToJson(this);
}

@JsonSerializable()
class GetMyUserProfile extends Query<UserProfileDTO> {
  @override
  String get endpointRoute => "UserProfile/GetMyUserProfile";

  @override
  UserProfileDTO deserializeResult(json) => UserProfileDTO.fromJson(json);

  @override
  Map<String, dynamic> toJson() => _$GetMyUserProfileToJson(this);
}

@JsonSerializable()
class CreateOrUpdateUserProfile extends Command {
  String firstName;
  String lastName;
  DateTime birthDate;

  @override
  String get endpointRoute => "UserProfile/CreateOrUpdateUserProfile";

  CreateOrUpdateUserProfile({this.firstName, this.lastName, this.birthDate});

  Map<String, dynamic> toJson() => _$CreateOrUpdateUserProfileToJson(this);
}
