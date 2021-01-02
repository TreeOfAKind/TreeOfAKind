import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:flutter/foundation.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

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
      yield* _handlePersonAdded(event.person);
    } else if (event is PersonUpdated) {
      yield* _handlePersonUpdated(event.person);
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<PeopleState> _handlePersonAdded(PersonDTO person) async* {
    yield const SavingState();

    final treeId = (treeBloc.state as PresentingTree).tree.treeId;

    final result =
        await peopleRepository.addPerson(treeId: treeId, person: person);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else if (!result.wasSuccessful) {
    } else {
      yield const PersonSavedSuccessfully();
      treeBloc.add(FetchTree(treeId));
    }
  }

  Stream<PeopleState> _handlePersonUpdated(PersonDTO person) async* {
    yield const SavingState();

    final treeId = (treeBloc.state as PresentingTree).tree.treeId;

    final result =
        await peopleRepository.updatePerson(treeId: treeId, person: person);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else if (!result.wasSuccessful) {
      yield ValidationErrorState(result.errorText);
    } else {
      yield const PersonSavedSuccessfully();
      treeBloc.add(FetchTree(treeId));
    }
  }
}
