part of 'tree_list_bloc.dart';

abstract class TreeListState {
  const TreeListState();
}

class LoadingState extends TreeListState {
  const LoadingState();
}

class PresentingData extends TreeListState {
  PresentingData(this.treeList);

  final List<TreeItemDTO> treeList;
}

class UnknownErrorState extends TreeListState {
  const UnknownErrorState();
}
