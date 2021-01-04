import 'package:bloc/bloc.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/foundation.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';

part 'person_files_event.dart';
part 'person_files_state.dart';

class PersonFilesBloc extends Bloc<PersonFilesEvent, PersonFilesState> {
  PersonFilesBloc(
      {@required this.treeId,
      @required this.personId,
      @required this.peopleRepository})
      : super(PresentingFiles());

  final String treeId;
  final String personId;
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
        treeId: treeId, personId: personId, file: event.file);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield const PresentingFiles();
    }
  }

  Stream<PersonFilesState> _handleFileDeleted(FileDeleted event) async* {
    yield LoadingState(event.fileId);

    final result = await peopleRepository.removePersonsFile(
        treeId: treeId, personId: personId, fileId: event.fileId);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield const PresentingFiles();
    }
  }
}
