import 'package:bloc/bloc.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/foundation.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';

part 'person_files_event.dart';
part 'person_files_state.dart';

class PersonFilesBloc extends Bloc<PersonFilesEvent, PersonFilesState> {
  PersonFilesBloc(
      {@required this.treeId,
      @required this.person,
      @required this.peopleRepository})
      : super(PresentingFiles(person.files));

  final String treeId;
  PersonDTO person;
  final PeopleRepository peopleRepository;

  @override
  Stream<PersonFilesState> mapEventToState(
    PersonFilesEvent event,
  ) async* {
    if (event is FileAdded) {
      yield* _handleFileAdded(event);
    } else if (event is FileDeleted) {
      yield* _handleFileDeleted(event);
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<PersonFilesState> _handleFileAdded(FileAdded event) async* {
    yield const LoadingState(null);

    final result = await peopleRepository.addPersonsFile(
        treeId: treeId, personId: person.id, file: event.file);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield* _fetchFiles();
    }
  }

  Stream<PersonFilesState> _handleFileDeleted(FileDeleted event) async* {
    yield LoadingState(event.fileId);

    final result = await peopleRepository.removePersonsFile(
        treeId: treeId, personId: person.id, fileId: event.fileId);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield* _fetchFiles();
    }
  }

  Stream<PersonFilesState> _fetchFiles() async* {
    // final result = await peopleRepository.fetchPerson();

    // if (result.unexpectedError) {
    //   yield const UnknownErrorState();
    // } else {
    //   person = result.data;
    //   yield PresentingFiles(_treeList);
    // }

    yield PresentingFiles(person.files);
  }
}
