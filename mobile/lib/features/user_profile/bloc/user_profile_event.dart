part of 'user_profile_bloc.dart';

abstract class UserProfileEvent {
  const UserProfileEvent();
}

class FetchUserProfile extends UserProfileEvent {
  const FetchUserProfile();
}

class SaveButtonClicked extends UserProfileEvent {
  SaveButtonClicked(this.userProfile);

  final UserProfileDTO userProfile;
}

class UserProfileFieldChanged extends UserProfileEvent {
  UserProfileFieldChanged(this.userProfile);

  final UserProfileDTO userProfile;
}
