import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/features/user_profile/bloc/user_profile_bloc.dart';

import 'avatar.dart';

class UserProfileView extends StatelessWidget {
  UserProfileView(
      {@required this.user,
      @required this.userProfile,
      @required this.canSave});

  final User user;
  final UserProfileDTO userProfile;
  final bool canSave;

  @override
  Widget build(BuildContext context) {
    final textTheme = Theme.of(context).textTheme;
    // ignore: close_sinks
    final bloc = BlocProvider.of<UserProfileBloc>(context);

    return Align(
      alignment: const Alignment(0, -1 / 3),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: <Widget>[
          Avatar(photo: user.photoURL),
          const SizedBox(height: 4.0),
          Text(user.email, style: textTheme.headline6),
          const SizedBox(height: 8.0),
          TextFormField(
            initialValue: userProfile.firstName,
            onChanged: (firstName) => bloc.add(UserProfileFieldChanged(
                UserProfileDTO(
                    id: userProfile.id,
                    firstName: firstName,
                    lastName: userProfile.lastName,
                    birthDate: userProfile.birthDate))),
            decoration: const InputDecoration(
              labelText: 'firstname',
              helperText: '',
            ),
          ),
          const SizedBox(height: 4.0),
          TextFormField(
            initialValue: userProfile.lastName,
            onChanged: (lastName) => bloc.add(UserProfileFieldChanged(
                UserProfileDTO(
                    id: userProfile.id,
                    firstName: userProfile.firstName,
                    lastName: lastName,
                    birthDate: userProfile.birthDate))),
            decoration: const InputDecoration(
              labelText: 'lastname',
              helperText: '',
            ),
          ),
          const SizedBox(height: 8.0),
          RaisedButton(
            child: const Text('SAVE'),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(30.0),
            ),
            color: const Color(0xFFFFD600),
            onPressed: () =>
                canSave ? bloc.add(SaveButtonClicked(userProfile)) : null,
          ),
        ],
      ),
    );
  }
}

class UserProfileTextField<T> extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Container();
  }
}
