import 'package:meta/meta.dart';
import 'package:tree_of_a_kind/contracts/common/base_repository.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_client.dart';

class UserProfileRepository extends BaseRepository {
  UserProfileRepository(CqrsClient cqrs) : super(cqrs);

  Future<BaseQueryResult<UserProfileDTO>> getMyUserProfile() async {
    final result = await get(GetMyUserProfile());

    return BaseQueryResult(result.data,
        unexpectedError: result.unexpectedError || result.data == null);
  }

  Future<BaseCommandResult> updateMyUserProfile(
      {@required String firstName,
      @required String lastName,
      @required DateTime birthDate}) {
    return run(CreateOrUpdateUserProfile()
      ..firstName = firstName
      ..lastName = lastName
      ..birthDate = birthDate);
  }
}
