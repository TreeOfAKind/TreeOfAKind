import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/features/home/bloc/tree_list_bloc.dart';

class AddTreeDialog extends StatefulWidget {
  AddTreeDialog(this._bloc);

  final TreeListBloc _bloc;

  @override
  _AddTreeDialogState createState() => _AddTreeDialogState(_bloc);
}

class _AddTreeDialogState extends State<AddTreeDialog> {
  _AddTreeDialogState(this.bloc);

  final TreeListBloc bloc;
  final controller = TextEditingController();
  final treeNameFieldKey = GlobalKey<FormState>();

  PlatformFile _selectedTreePhoto;

  Future<void> _pickTreePhoto() async {
    final result = await FilePicker.platform.pickFiles(
      type: FileType.custom,
      allowedExtensions: ['jpg', 'jpeg', 'png'],
    );

    setState(() {
      _selectedTreePhoto = result?.files?.single;
    });
  }

  @override
  Widget build(BuildContext context) {
    return new AlertDialog(
      title: Text('Add your new family tree'),
      titleTextStyle: Theme.of(context).textTheme.headline4,
      content: Form(
          key: treeNameFieldKey,
          child: Column(mainAxisSize: MainAxisSize.min, children: [
            TextFormField(
              controller: controller,
              validator: (text) =>
                  text.isEmpty ? 'Please provide family tree title' : null,
              decoration: const InputDecoration(
                  labelText: 'Title', helperText: 'Your new family tree title'),
            ),
            SizedBox(height: 8.0),
            RaisedButton(
                onPressed: () => _pickTreePhoto(),
                child: Text(
                  _selectedTreePhoto == null
                      ? 'Add a family tree picture, if you wish.'
                      : _selectedTreePhoto.name,
                  overflow: TextOverflow.ellipsis,
                ))
          ])),
      actions: <Widget>[
        TextButton(
          child: Text('Cancel'),
          onPressed: () {
            Navigator.of(context).pop();
          },
        ),
        TextButton(
          child: Text('Add'),
          onPressed: () {
            if (treeNameFieldKey.currentState.validate()) {
              Navigator.of(context).pop();
              bloc.add(SaveNewTree(controller.text));
              _selectedTreePhoto = null;
            }
          },
        ),
      ],
    );
  }
}
