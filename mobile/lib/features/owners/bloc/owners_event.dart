part of 'owners_bloc.dart';

abstract class OwnersEvent {
  const OwnersEvent();
}

class FetchMyProfile extends OwnersEvent {
  const FetchMyProfile();
}

class AddOwner extends OwnersEvent {
  const AddOwner(this.userEmail);

  final String userEmail;
}

class RemoveOwner extends OwnersEvent {
  const RemoveOwner(this.userId);

  final String userId;
}
