import 'dart:async';

import 'package:bloc/bloc.dart';
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
    } else if (event is OpenNewTreeForm) {
      yield PresentingNewTreeForm(_treeList);
    } else if (event is CloseNewTreeForm) {
      yield PresentingList(_treeList);
    } else if (event is SaveNewTree) {
      yield* _handleSaveNewTree(event.treeName);
    } else if (event is DeleteTree) {
      yield* _handleDeleteTree(event.treeId);
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<TreeListState> _handleFetchTreeList() async* {
    yield _treeList == null
        ? const InitialLoadingState()
        : RefreshLoadingState(_treeList, null);

    final result = await treeRepository.getMyTrees();

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      _treeList = result.data;
      yield PresentingList(_treeList);
    }
  }

  Stream<TreeListState> _handleSaveNewTree(String treeName) async* {
    yield RefreshLoadingState(_treeList, null);

    final result = await treeRepository.addTree(treeName: treeName);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      final queryResult = await treeRepository.getMyTrees();

      if (queryResult.unexpectedError) {
        yield const UnknownErrorState();
      } else {
        _treeList = queryResult.data;
        yield PresentingList(_treeList);
      }
    }
  }

  Stream<TreeListState> _handleDeleteTree(String treeId) async* {
    yield RefreshLoadingState(_treeList, treeId);

    final result = await treeRepository.deleteTree(treeId: treeId);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      final queryResult = await treeRepository.getMyTrees();

      if (queryResult.unexpectedError) {
        yield const UnknownErrorState();
      } else {
        _treeList = queryResult.data;
        yield PresentingList(_treeList);
      }
    }
  }
}
