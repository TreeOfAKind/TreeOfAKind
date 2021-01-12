import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/home/bloc/tree_list_bloc.dart';

class AddOrUpdateTreeDialog extends StatefulWidget {
  AddOrUpdateTreeDialog(this.bloc, {this.tree});

  final TreeListBloc bloc;
  final TreeItemDTO tree;

  @override
  _AddOrUpdateTreeDialogState createState() => _AddOrUpdateTreeDialogState();
}

class _AddOrUpdateTreeDialogState extends State<AddOrUpdateTreeDialog> {
  final controller = TextEditingController();
  final formKey = GlobalKey<FormState>();

  PlatformFile _selectedTreePhoto;

  bool _isUpdate() => widget.tree != null;
  bool _photoDeleted = false;

  Future<void> _pickTreePhoto() async {
    final result = await FilePicker.platform.pickFiles(
      type: FileType.custom,
      allowedExtensions: ['jpg', 'jpeg', 'png'],
    );

    setState(() {
      _photoDeleted = false;
      _selectedTreePhoto = result?.files?.single;
    });
  }

  Widget _getTreePhoto(BuildContext context) {
    final theme = Theme.of(context);

    final placeholder =
        Icon(Icons.nature, color: theme.accentColor, size: 70.0);

    if (_photoDeleted) {
      return placeholder;
    } else if (_selectedTreePhoto != null) {
      return Image.file(File(_selectedTreePhoto.path));
    } else if (widget.tree?.photoUri != null) {
      return Image.network(widget.tree.photoUri);
    } else {
      return placeholder;
    }
  }

  @override
  void initState() {
    super.initState();

    controller.text = widget.tree?.treeName;
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return new AlertDialog(
      title: Text(
          _isUpdate() ? "Update your family tree" : 'Add your new family tree'),
      titleTextStyle: Theme.of(context).textTheme.headline4,
      content: Form(
          key: formKey,
          child: Column(mainAxisSize: MainAxisSize.min, children: [
            Row(
              children: [
                Expanded(
                  child: Padding(
                    padding: const EdgeInsets.only(left: 50.0),
                    child: FlatButton(
                      onPressed: () => _pickTreePhoto(),
                      child: ClipRRect(
                        borderRadius: BorderRadius.circular(20.0), //or 15.0
                        child: Container(
                          height: 70.0,
                          width: 70.0,
                          color: _getTreePhoto(context) == null
                              ? theme.backgroundColor
                              : null,
                          child: _getTreePhoto(context),
                        ),
                      ),
                    ),
                  ),
                ),
                IconButton(
                    iconSize: 20.0,
                    icon: Icon(Icons.clear),
                    onPressed: () => setState(() {
                          _photoDeleted = true;
                          _selectedTreePhoto = null;
                        }))
              ],
            ),
            SizedBox(height: 16.0),
            TextFormField(
              controller: controller,
              validator: (text) =>
                  text.isEmpty ? 'Please provide family tree title' : null,
              decoration: InputDecoration(
                  labelText: 'Title',
                  helperText:
                      'Your${_isUpdate() ? '' : ' new'} family tree title'),
            ),
          ])),
      actions: <Widget>[
        TextButton(
          child: Text('Cancel'),
          onPressed: () {
            Navigator.of(context).pop();
          },
        ),
        TextButton(
          child: Text(_isUpdate() ? 'Update' : 'Add'),
          onPressed: () {
            if (formKey.currentState.validate()) {
              if (_isUpdate()) {
                widget.bloc.add(UpdateTree(widget.tree.id,
                    treeName: controller.text,
                    treePhoto: _selectedTreePhoto,
                    updatePhoto:
                        (_photoDeleted && widget.tree?.photoUri != null) ||
                            _selectedTreePhoto != null));
              } else {
                widget.bloc
                    .add(SaveNewTree(controller.text, _selectedTreePhoto));
              }
              Navigator.of(context).pop();
              _selectedTreePhoto = null;
            }
          },
        ),
      ],
    );
  }
}
