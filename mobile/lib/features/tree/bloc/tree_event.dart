part of 'tree_bloc.dart';

abstract class TreeEvent {
  const TreeEvent();
}

class FetchTree extends TreeEvent {
  const FetchTree(this.treeId);

  final String treeId;
}
