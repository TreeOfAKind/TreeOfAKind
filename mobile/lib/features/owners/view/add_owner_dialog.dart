import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/features/authentication/models/email.dart';
import 'package:tree_of_a_kind/features/owners/bloc/owners_bloc.dart';

class AddOwnerDialog extends StatefulWidget {
  AddOwnerDialog(this.bloc);

  final OwnersBloc bloc;

  @override
  _AddOwnerDialogState createState() => _AddOwnerDialogState();
}

class _AddOwnerDialogState extends State<AddOwnerDialog> {
  final emailController = TextEditingController();
  final formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return new AlertDialog(
      title: Text('Enter email of a user you wish to share you tree with'),
      titleTextStyle: Theme.of(context).textTheme.headline4,
      content: Form(
          key: formKey,
          child: TextFormField(
            controller: emailController,
            validator: (text) =>
                Email.validate(text) ? null : 'Please provide a valid email',
            decoration: const InputDecoration(
              labelText: 'Email',
            ),
          )),
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
            if (formKey.currentState.validate()) {
              Navigator.of(context).pop();
              widget.bloc.add(AddOwner(emailController.text));
            }
          },
        ),
      ],
    );
  }
}
