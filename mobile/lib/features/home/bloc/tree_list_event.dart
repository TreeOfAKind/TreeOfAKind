part of 'tree_list_bloc.dart';

abstract class TreeListEvent {
  const TreeListEvent();
}

class FetchTreeList extends TreeListEvent {
  const FetchTreeList();
}
