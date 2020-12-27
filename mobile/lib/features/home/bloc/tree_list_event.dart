part of 'tree_list_bloc.dart';

abstract class TreeListEvent {
  const TreeListEvent();
}

class FetchTreeList extends TreeListEvent {
  const FetchTreeList();
}

class SaveNewTree extends TreeListEvent {
  const SaveNewTree(this.treeName, this.treePhoto);

  final String treeName;
  final PlatformFile treePhoto;
}

class DeleteTree extends TreeListEvent {
  const DeleteTree(this.treeId);

  final String treeId;
}
