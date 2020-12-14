part of 'tree_list_bloc.dart';

abstract class TreeListEvent {
  const TreeListEvent();
}

class FetchTreeList extends TreeListEvent {
  const FetchTreeList();
}

class OpenNewTreeForm extends TreeListEvent {
  const OpenNewTreeForm();
}

class CloseNewTreeForm extends TreeListEvent {
  const CloseNewTreeForm();
}

class SaveNewTree extends TreeListEvent {
  const SaveNewTree(this.treeName);

  final String treeName;
}

class DeleteTree extends TreeListEvent {
  const DeleteTree(this.treeId);

  final String treeId;
}
