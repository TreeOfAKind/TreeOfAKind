import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:meta/meta.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';

part 'tree_list_event.dart';
part 'tree_list_state.dart';

class TreeListBloc extends Bloc<TreeListEvent, TreeListState> {
  TreeListBloc({@required this.treeRepository}) : super(LoadingState());

  final TreeRepository treeRepository;

  @override
  Stream<TreeListState> mapEventToState(
    TreeListEvent event,
  ) async* {
    if (event is FetchTreeList) {
      yield* _handleFetchTreeList();
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<TreeListState> _handleFetchTreeList() async* {
    yield const LoadingState();
    final result = await treeRepository.getMyTrees();
    final treeList = result.unexpectedError ? null : result.data;
    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield PresentingData(treeList);
    }
  }
}
