part of 'tree_bloc.dart';

abstract class TreeEvent {
  const TreeEvent();
}

class FetchTree extends TreeEvent {
  const FetchTree(this.treeId);

  final String treeId;
}

class PersonAdded extends TreeEvent {
  const PersonAdded(this.person);

  final PersonDTO person;
}

class PersonUpdated extends TreeEvent {
  const PersonUpdated(this.person);

  final PersonDTO person;
}

class PersonRemoved extends TreeEvent {
  const PersonRemoved(this.personId);

  final String personId;
}

class RemovePerson extends TreeEvent {
  const RemovePerson(this.personId);

  final String personId;
}
