import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/foundation.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';
import 'package:tree_of_a_kind/main_dev.dart';

part 'people_event.dart';
part 'people_state.dart';

class PeopleBloc extends Bloc<PeopleEvent, PeopleState> {
  PeopleBloc({@required this.treeBloc, @required this.peopleRepository})
      : super(PresentingForm());

  // ignore: close_sinks
  final TreeBloc treeBloc;
  final PeopleRepository peopleRepository;

  @override
  Stream<PeopleState> mapEventToState(
    PeopleEvent event,
  ) async* {
    if (event is PersonAdded) {
      yield* _handlePersonAdded(event);
    } else if (event is PersonUpdated) {
      yield* _handlePersonUpdated(event);
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<PeopleState> _handlePersonAdded(PersonAdded event) async* {
    yield const SavingState();

    final treeId = (treeBloc.state as PresentingTree).tree.treeId;

    var result =
        await peopleRepository.addPerson(treeId: treeId, person: event.person);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else if (!result.wasSuccessful) {
    } else {
      if (event.mainPhoto != null) {
        result = await peopleRepository.updatePersonsMainPhoto(
            treeId: treeId, personId: result.entityId, file: event.mainPhoto);
      }

      if (result.unexpectedError) {
        yield const UnknownErrorState();
      } else {
        yield const PersonSavedSuccessfully();
        treeBloc.add(FetchTree(treeId));
      }
    }
  }

  Stream<PeopleState> _handlePersonUpdated(PersonUpdated event) async* {
    yield const SavingState();

    final treeId = (treeBloc.state as PresentingTree).tree.treeId;

    var result = await peopleRepository.updatePerson(
        treeId: treeId, person: event.person);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else if (!result.wasSuccessful) {
      yield ValidationErrorState(result.errorText);
    } else {
      if (event.deletePhoto && event.person.mainPhoto?.id != null) {
        result = await peopleRepository.removePersonsFile(
            treeId: treeId,
            personId: result.entityId,
            fileId: event.person.mainPhoto.id);
      } else if (event.mainPhoto != null) {
        result = await peopleRepository.updatePersonsMainPhoto(
            treeId: treeId, personId: result.entityId, file: event.mainPhoto);
      }

      if (result.unexpectedError) {
        yield const UnknownErrorState();
      } else {
        yield const PersonSavedSuccessfully();
        treeBloc.add(FetchTree(treeId));
      }
    }
  }
}
