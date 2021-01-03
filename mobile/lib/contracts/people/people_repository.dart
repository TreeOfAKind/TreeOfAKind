import 'package:dio/dio.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/foundation.dart';
import 'package:mime/mime.dart';
import 'package:tree_of_a_kind/contracts/common/base_repository.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_client.dart';
import 'package:http_parser/http_parser.dart';

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

  Future<BaseCommandResult> removePerson(
      {@required String treeId, @required String personId}) {
    return run(RemovePerson()
      ..treeId = treeId
      ..personId = personId);
  }

  Future<BaseCommandResult> updatePersonsMainPhoto(
      {@required String treeId,
      @required String personId,
      @required PlatformFile file}) async {
    return runWithFile(AddOrChangePersonsMainPhoto(
        treeId: treeId,
        personId: personId,
        file: await MultipartFile.fromFile(file.path,
            filename: file.name,
            contentType: MediaType.parse(lookupMimeType(file.name)))));
  }

  Future<BaseCommandResult> addPersonsFile(
      {@required String treeId,
      @required String personId,
      @required PlatformFile file}) async {
    return runWithFile(AddPersonsFile(
        treeId: treeId,
        personId: personId,
        file: await MultipartFile.fromFile(file.path,
            filename: file.name,
            contentType: MediaType.parse(lookupMimeType(file.name)))));
  }

  Future<BaseCommandResult> removePersonsFile(
      {@required String treeId,
      @required String personId,
      @required String fileId}) async {
    return run(
        RemovePersonsFile(treeId: treeId, personId: personId, fileId: fileId));
  }
}
