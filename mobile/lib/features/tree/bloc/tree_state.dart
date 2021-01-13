part of 'tree_bloc.dart';

abstract class TreeState {
  const TreeState();
}

class LoadingState extends TreeState {
  const LoadingState();
}

class PresentingTree extends TreeState {
  const PresentingTree(this.tree);

  final TreeDTO tree;
}

class UnknownErrorState extends TreeState {
  const UnknownErrorState();
}
