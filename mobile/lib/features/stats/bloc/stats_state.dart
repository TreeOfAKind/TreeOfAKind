part of 'stats_bloc.dart';

abstract class StatsState {
  const StatsState();
}

class LoadingState extends StatsState {
  const LoadingState();
}

class PresentingStats extends StatsState {
  const PresentingStats(this.stats);

  final TreeStatsDTO stats;
}

class UnknownErrorState extends StatsState {
  const UnknownErrorState();
}
