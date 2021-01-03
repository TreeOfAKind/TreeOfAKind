part of 'tree_bloc.dart';

abstract class TreeState {
  const TreeState();
}

class LoadingState extends TreeState {
  const LoadingState();
}

class PresentingTree extends TreeState {
  const PresentingTree(this.tree, this.treeGraph);

  final TreeDTO tree;
  final List<NodeInput> treeGraph;
}

class UnknownErrorState extends TreeState {
  const UnknownErrorState();
}
