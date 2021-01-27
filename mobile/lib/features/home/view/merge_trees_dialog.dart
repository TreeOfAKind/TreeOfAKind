import 'package:flutter/material.dart';
import 'package:flutter_typeahead/flutter_typeahead.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/home/bloc/tree_list_bloc.dart';

class MergeTreesDialog extends StatefulWidget {
  MergeTreesDialog(
      {@required this.firstTreeId,
      @required this.treesList,
      @required this.bloc});

  final String firstTreeId;
  final List<TreeItemDTO> treesList;
  final TreeListBloc bloc;

  @override
  _MergeTreesDialogState createState() => _MergeTreesDialogState();
}

class _MergeTreesDialogState extends State<MergeTreesDialog> {
  final controller = TextEditingController();
  final formKey = GlobalKey<FormState>();

  Iterable<TreeItemDTO> _getPossibleTrees() => widget.treesList.where((tree) =>
      tree.id != widget.firstTreeId && tree.treeName == controller.text);

  @override
  Widget build(BuildContext context) {
    return new AlertDialog(
      title: Text(
          'Merge other family tree with ${widget.treesList.firstWhere((treeItem) => treeItem.id == widget.firstTreeId).treeName}'),
      titleTextStyle: Theme.of(context).textTheme.headline4,
      content: Form(
          key: formKey,
          child: TypeAheadFormField<TreeItemDTO>(
              textFieldConfiguration: TextFieldConfiguration(
                  controller: controller,
                  decoration: new InputDecoration(
                    labelText: "Other tree",
                    suffixIcon: FlatButton(
                        child: const Icon(Icons.clear),
                        onPressed: () => controller.clear()),
                  )),
              validator: (text) => text.isNotEmpty &&
                      _getPossibleTrees().isEmpty
                  ? 'Provide full name of other family tree you want to merge'
                  : null,
              onSuggestionSelected: (tree) =>
                  setState(() => controller.text = tree.treeName),
              itemBuilder: (context, tree) =>
                  ListTile(title: Text(tree.treeName)),
              suggestionsCallback: (query) => query?.isEmpty ?? true
                  ? List()
                  : widget.treesList
                      .where((tree) =>
                          widget.firstTreeId != tree.id &&
                          tree.treeName
                              .toLowerCase()
                              .contains(query.toLowerCase()))
                      .take(5)
                      .toList()
                ..sort((tree1, tree2) =>
                    tree1.treeName.compareTo(tree2.treeName)))),
      actions: <Widget>[
        TextButton(
          child: Text('Cancel'),
          onPressed: () {
            Navigator.of(context).pop();
          },
        ),
        TextButton(
          child: Text('Merge trees'),
          onPressed: () {
            if (formKey.currentState.validate()) {
              Navigator.of(context).pop();
              widget.bloc.add(TreesMerged(
                  widget.firstTreeId, _getPossibleTrees().first.id));
            }
          },
        ),
      ],
    );
  }
}
