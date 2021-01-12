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

class UpdateTree extends TreeListEvent {
  const UpdateTree(this.treeId,
      {@required this.treeName, this.treePhoto, this.updatePhoto = false});

  final String treeId;
  final String treeName;
  final PlatformFile treePhoto;
  final bool updatePhoto;
}

class DeleteTree extends TreeListEvent {
  const DeleteTree(this.treeId);

  final String treeId;
}

class TreesMerged extends TreeListEvent {
  const TreesMerged(this.firstTreeId, this.secondTreeId);

  final String firstTreeId;
  final String secondTreeId;
}
