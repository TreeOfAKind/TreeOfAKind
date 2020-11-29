part of 'user_profile_bloc.dart';

abstract class UserProfileState {
  const UserProfileState();
}

class LoadingState extends UserProfileState {
  const LoadingState();
}

class UnchangedData extends UserProfileState {
  UnchangedData(this.userProfile);

  final UserProfileDTO userProfile;
}

class ChangedData extends UserProfileState {
  ChangedData(this.userProfile);

  final UserProfileDTO userProfile;
}

class UnknownErrorState extends UserProfileState {
  const UnknownErrorState();
}
