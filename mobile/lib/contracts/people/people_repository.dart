import 'package:flutter/foundation.dart';
import 'package:tree_of_a_kind/contracts/common/base_repository.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_client.dart';

class PeopleRepository extends BaseRepository {
  PeopleRepository(CqrsClient cqrs) : super(cqrs);

  Future<BaseCommandResult> addPerson(
      {@required String treeId, @required PersonDTO person}) {
    return run(AddPerson(
        treeId: treeId,
        name: person.name,
        lastName: person.lastName,
        gender: person.gender,
        birthDate: person.birthDate,
        deathDate: person.deathDate,
        description: person.description,
        biography: person.biography,
        mother: person.mother,
        father: person.father,
        spouse: person.spouse));
  }

  Future<BaseCommandResult> updatePerson(
      {@required String treeId, @required PersonDTO person}) {
    return run(UpdatePerson(
        treeId: treeId,
        personId: person.id,
        name: person.name,
        lastName: person.lastName,
        gender: person.gender,
        birthDate: person.birthDate,
        deathDate: person.deathDate,
        description: person.description,
        biography: person.biography,
        mother: person.mother,
        father: person.father,
        spouse: person.spouse));
  }

  Future<BaseCommandResult> deleteTree(
      {@required String treeId, @required String personId}) {
    return run(RemovePerson()
      ..treeId = treeId
      ..personId = personId);
  }
}
