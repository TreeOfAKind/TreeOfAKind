import 'dart:async';

import 'package:authentication_repository/authentication_repository.dart';
import 'package:bloc/bloc.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/foundation.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/contracts/user_profile/user_profile_repository.dart';
import 'package:tree_of_a_kind/features/authentication/bloc/authentication_bloc.dart';

part 'user_profile_event.dart';
part 'user_profile_state.dart';

class UserProfileBloc extends Bloc<UserProfileEvent, UserProfileState> {
  UserProfileBloc({@required this.user, @required this.userProfileRepository})
      : super(LoadingState());

  final User user;
  final UserProfileRepository userProfileRepository;
  UserProfileDTO _userProfile;

  @override
  Stream<UserProfileState> mapEventToState(
    UserProfileEvent event,
  ) async* {
    if (event is FetchUserProfile) {
      yield* _handleFetchUserProfile();
    } else if (event is UserProfileFieldChanged) {
      _userProfile = event.userProfile;
      yield ChangedData(_userProfile);
    } else if (event is SaveButtonClicked) {
      yield* _handleSaveButtonClicked();
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<UserProfileState> _handleFetchUserProfile() async* {
    yield const LoadingState();
    final result = await userProfileRepository.getMyUserProfile();
    _userProfile = result.unexpectedError ? null : result.data;
    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield UnchangedData(_userProfile);
    }
  }

  Stream<UserProfileState> _handleSaveButtonClicked() async* {
    yield const LoadingState();
    final result = await userProfileRepository.updateMyUserProfile(
        firstName: _userProfile.firstName,
        lastName: _userProfile.lastName,
        birthDate: _userProfile.birthDate);
    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield UnchangedData(_userProfile);
    }
  }
}
