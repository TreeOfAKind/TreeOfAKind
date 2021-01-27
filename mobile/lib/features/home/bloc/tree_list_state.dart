part of 'tree_list_bloc.dart';

abstract class TreeListState {
  const TreeListState();
}

class InitialLoadingState extends TreeListState {
  const InitialLoadingState();
}

class RefreshLoadingState extends TreeListState {
  const RefreshLoadingState(this.treeList, this.deletedTreeId);

  final List<TreeItemDTO> treeList;
  final String deletedTreeId;
}

class PresentingList extends TreeListState {
  const PresentingList(this.treeList);

  final List<TreeItemDTO> treeList;
}

class PresentingNewTreeForm extends TreeListState {
  const PresentingNewTreeForm(this.treeList);

  final List<TreeItemDTO> treeList;
}

class ValidationErrorState extends TreeListState {
  const ValidationErrorState(this.errorText);

  final String errorText;
}

class UnknownErrorState extends TreeListState {
  const UnknownErrorState();
}
