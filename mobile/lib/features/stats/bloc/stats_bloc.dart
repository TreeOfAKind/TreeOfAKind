import 'package:bloc/bloc.dart';
import 'package:flutter/foundation.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';

part 'stats_event.dart';
part 'stats_state.dart';

class StatsBloc extends Bloc<StatsEvent, StatsState> {
  StatsBloc({@required this.treeRepository}) : super(LoadingState());

  final TreeRepository treeRepository;

  @override
  Stream<StatsState> mapEventToState(
    StatsEvent event,
  ) async* {
    if (event is FetchStats) {
      yield* _handleFetchStats(event.treeId);
    } else {
      throw new Exception("Unhandled event.");
    }
  }

  Stream<StatsState> _handleFetchStats(String treeId) async* {
    yield const LoadingState();

    final result = await treeRepository.getTreeStats(treeId: treeId);

    if (result.unexpectedError) {
      yield const UnknownErrorState();
    } else {
      yield PresentingStats(result.data);
    }
  }
}
