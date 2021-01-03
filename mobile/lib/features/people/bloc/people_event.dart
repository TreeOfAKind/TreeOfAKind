part of 'people_bloc.dart';

abstract class PeopleEvent {
  const PeopleEvent();
}

class PersonAdded extends PeopleEvent {
  const PersonAdded(this.person);

  final PersonDTO person;
}

class PersonUpdated extends PeopleEvent {
  const PersonUpdated(this.person);

  final PersonDTO person;
}
