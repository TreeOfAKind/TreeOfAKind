part of 'people_bloc.dart';

abstract class PeopleEvent {
  const PeopleEvent();
}

class PersonAdded extends PeopleEvent {
  const PersonAdded(this.person, this.mainPhoto);

  final PersonDTO person;
  final PlatformFile mainPhoto;
}

class PersonUpdated extends PeopleEvent {
  const PersonUpdated(this.person, this.mainPhoto, this.deletePhoto);

  final PersonDTO person;
  final PlatformFile mainPhoto;
  final bool deletePhoto;
}
