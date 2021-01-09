part of 'owners_bloc.dart';

abstract class OwnersState {
  const OwnersState();
}

class PresentingOwners extends OwnersState {
  const PresentingOwners(this.owners);

  final List<UserProfileDTO> owners;
}

class LoadingState extends OwnersState {
  const LoadingState(this.deletedUserId);

  final String deletedUserId;
}

class ValidationErrorState extends OwnersState {
  const ValidationErrorState(this.errorText);

  final String errorText;
}

class UnknownErrorState extends OwnersState {
  const UnknownErrorState();
}
