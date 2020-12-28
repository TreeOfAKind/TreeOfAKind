import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/features/user_profile/bloc/user_profile_bloc.dart';

import '../../common/avatar.dart';

class UserProfileView extends StatelessWidget {
  UserProfileView(
      {@required this.user,
      @required this.userProfile,
      @required this.canSave});

  final User user;
  final UserProfileDTO userProfile;
  final bool canSave;

  Future<void> _selectDate(BuildContext context, UserProfileBloc bloc) async {
    final DateTime picked = await showDatePicker(
      context: context,
      initialDate: userProfile.birthDate,
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
      initialEntryMode: DatePickerEntryMode.input,
      helpText: 'Select your birthdate',
    );

    if (picked != null) {
      bloc.add(UserProfileFieldChanged(UserProfileDTO(
          id: userProfile.id,
          firstName: userProfile.firstName,
          lastName: userProfile.lastName,
          birthDate: picked)));
    }
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    // ignore: close_sinks
    final bloc = BlocProvider.of<UserProfileBloc>(context);

    return Align(
      alignment: const Alignment(0, -1 / 3),
      child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(mainAxisSize: MainAxisSize.min, children: <Widget>[
            Avatar(photo: user.photoURL, avatarSize: 48),
            const SizedBox(height: 4.0),
            Text(user.email, style: theme.textTheme.headline6),
            const SizedBox(height: 8.0),
            TextFormField(
              key: Key('userProfile_firstname_textFormField'),
              initialValue: userProfile.firstName,
              onChanged: (firstName) => bloc.add(UserProfileFieldChanged(
                  UserProfileDTO(
                      id: userProfile.id,
                      firstName: firstName,
                      lastName: userProfile.lastName,
                      birthDate: userProfile.birthDate))),
              decoration: const InputDecoration(
                labelText: 'firstname',
              ),
            ),
            const SizedBox(height: 8.0),
            TextFormField(
              key: Key('userProfile_lastname_textFormField'),
              initialValue: userProfile.lastName,
              onChanged: (lastName) => bloc.add(UserProfileFieldChanged(
                  UserProfileDTO(
                      id: userProfile.id,
                      firstName: userProfile.firstName,
                      lastName: lastName,
                      birthDate: userProfile.birthDate))),
              decoration: const InputDecoration(
                labelText: 'lastname',
              ),
            ),
            const SizedBox(height: 8.0),
            Row(mainAxisAlignment: MainAxisAlignment.spaceEvenly, children: [
              Text(
                'birthdate:',
                style: theme.textTheme.bodyText1,
              ),
              const SizedBox(width: 20.0),
              RaisedButton(
                  key: Key('userProfile_birthdate_raisedButton'),
                  onPressed: () => _selectDate(context, bloc),
                  child:
                      Text("${userProfile.birthDate.toLocal()}".split(' ')[0])),
            ]),
            const SizedBox(height: 8.0),
            RaisedButton(
              key: Key('userProfile_save_raisedButton'),
              child: const Text('SAVE'),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(30.0),
              ),
              color: theme.accentColor,
              disabledColor: theme.disabledColor,
              onPressed: () =>
                  canSave ? bloc.add(SaveButtonClicked(userProfile)) : null,
            ),
          ])),
    );
  }
}
