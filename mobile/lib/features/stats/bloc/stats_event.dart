part of 'stats_bloc.dart';

abstract class StatsEvent {
  const StatsEvent();
}

class FetchStats extends StatsEvent {
  const FetchStats(this.treeId);

  final String treeId;
}
