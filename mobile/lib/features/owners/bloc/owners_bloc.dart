import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:flutter/foundation.dart';
import 'package:graphite/graphite.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/contracts/user_profile/user_profile_repository.dart';

part 'owners_event.dart';
part 'owners_state.dart';

class OwnersBloc extends Bloc<OwnersEvent, OwnersState> {
  OwnersBloc(
      {@required this.treeRepository,
      @required this.userProfileRepository,
      @required this.tree})
      : super(LoadingState(null));

  final TreeRepository treeRepository;
  final UserProfileRepository userProfileRepository;

  UserProfileDTO myUserProfile;
  TreeDTO tree;

  @override
  Stream<OwnersState> mapEventToState(
    OwnersEvent event,
  ) async* {
    if (event is AddOwner) {
      yield* _handleAddOwner(event);
    } else if (event is RemoveOwner) {
      yield* _handleRemoveOwner(event);
    } else if (event is FetchMyProfile) {
      yield* _handleFetchMyProfile(event);
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<OwnersState> _handleFetchMyProfile(FetchMyProfile event) async* {
    yield LoadingState(null);

    final result = await userProfileRepository.getMyUserProfile();

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield PresentingOwners(_otherOwners());
    }
  }

  Stream<OwnersState> _handleAddOwner(AddOwner event) async* {
    yield LoadingState(null);

    final result = await treeRepository.addTreeOwner(
        treeId: tree.treeId, userEmail: event.userEmail);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else if (!result.wasSuccessful) {
      yield ValidationErrorState(result.errorText);
    } else {
      yield* _fetchOwners();
    }
  }

  Stream<OwnersState> _handleRemoveOwner(RemoveOwner event) async* {
    yield LoadingState(event.userId);

    final result = await treeRepository.removeTreeOwner(
        treeId: tree.treeId, userId: event.userId);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield* _fetchOwners();
    }
  }

  Stream<OwnersState> _fetchOwners() async* {
    final result = await treeRepository.getTreeDetails(treeId: tree.treeId);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      tree = result.data;
      yield PresentingOwners(_otherOwners());
    }
  }

  List<UserProfileDTO> _otherOwners() =>
      tree.owners.where((owner) => owner.id != myUserProfile.id).toList();
}
