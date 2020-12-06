part of 'user_profile_bloc.dart';

abstract class UserProfileState {
  const UserProfileState();
}

class LoadingState extends UserProfileState {
  const LoadingState();
}

class PresentingData extends UserProfileState {
  PresentingData(this.hasChanged, this.userProfile);

  final bool hasChanged;
  final UserProfileDTO userProfile;
}

class UnknownErrorState extends UserProfileState {
  const UnknownErrorState();
}
