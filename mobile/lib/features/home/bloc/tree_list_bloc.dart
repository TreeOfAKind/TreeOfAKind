import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:file_picker/file_picker.dart';
import 'package:meta/meta.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';

part 'tree_list_event.dart';
part 'tree_list_state.dart';

class TreeListBloc extends Bloc<TreeListEvent, TreeListState> {
  TreeListBloc({@required this.treeRepository}) : super(InitialLoadingState());

  final TreeRepository treeRepository;

  List<TreeItemDTO> _treeList;

  @override
  Stream<TreeListState> mapEventToState(
    TreeListEvent event,
  ) async* {
    if (event is FetchTreeList) {
      yield* _handleFetchTreeList();
    } else if (event is SaveNewTree) {
      yield* _handleSaveNewTree(event);
    } else if (event is UpdateTree) {
      yield* _handleUpdateTree(event);
    } else if (event is DeleteTree) {
      yield* _handleDeleteTree(event);
    } else if (event is MergeTrees) {
      yield* _handleMergeTrees(event);
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<TreeListState> _handleFetchTreeList({String deletedTreeId}) async* {
    yield _treeList == null
        ? const InitialLoadingState()
        : RefreshLoadingState(_treeList, deletedTreeId);

    final result = await treeRepository.getMyTrees();

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      _treeList = result.data;
      yield PresentingList(_treeList);
    }
  }

  Stream<TreeListState> _handleSaveNewTree(SaveNewTree event) async* {
    yield RefreshLoadingState(_treeList, null);

    var result = await treeRepository.addTree(treeName: event.treeName);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      if (event.treePhoto != null) {
        result = await treeRepository.updateTreePhoto(
            treeId: result.entityId, image: event.treePhoto);
      }

      if (result.unexpectedError) {
        yield const UnknownErrorState();
      } else {
        yield* _handleFetchTreeList();
      }
    }
  }

  Stream<TreeListState> _handleUpdateTree(UpdateTree event) async* {
    yield RefreshLoadingState(_treeList, null);

    var result = await treeRepository.updateTree(
        treeId: event.treeId, treeName: event.treeName);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      if (event.updatePhoto) {
        result = await treeRepository.updateTreePhoto(
            treeId: result.entityId, image: event.treePhoto);
      }

      if (result.unexpectedError) {
        yield const UnknownErrorState();
      } else {
        yield* _handleFetchTreeList();
      }
    }
  }

  Stream<TreeListState> _handleMergeTrees(MergeTrees event) async* {
    yield RefreshLoadingState(_treeList, null);

    final result = await treeRepository.mergeTrees(
        firstTreeId: event.firstTreeId, secondTreeId: event.secondTreeId);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else if (!result.wasSuccessful) {
      yield ValidationErrorState(result.errorText);
    } else {
      yield* _handleFetchTreeList();
    }
  }

  Stream<TreeListState> _handleDeleteTree(DeleteTree event) async* {
    yield RefreshLoadingState(_treeList, event.treeId);

    final result = await treeRepository.deleteTree(treeId: event.treeId);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield* _handleFetchTreeList(deletedTreeId: event.treeId);
    }
  }
}
