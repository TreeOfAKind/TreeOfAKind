import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/features/common/avatar.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class AddOrUpdatePersonView extends StatefulWidget {
  AddOrUpdatePersonView({@required this.treeId, this.person});
  final String treeId;
  final PersonDTO person;

  @override
  _AddOrUpdatePersonViewState createState() =>
      _AddOrUpdatePersonViewState(treeId: treeId, person: person);
}

class _AddOrUpdatePersonViewState extends State<AddOrUpdatePersonView> {
  _AddOrUpdatePersonViewState({@required this.treeId, this.person}) {
    if (person == null) {
      adding = true;
      person = PersonDTO();
    } else {
      adding = false;
    }
  }

  String treeId;
  PersonDTO person;
  bool adding;

  final formKey = GlobalKey<FormState>();

  String _mapGenderToString(Gender gender) {
    switch (gender) {
      case Gender.unknown:
        return 'Unknown';
        break;
      case Gender.male:
        return 'Male';
        break;
      case Gender.female:
        return 'Female';
        break;
      case Gender.other:
        return 'Other';
        break;
      default:
        return 'Unknown';
    }
  }

  Gender _mapStringToGender(String string) {
    return Gender.values.firstWhere(
        (gender) => _mapGenderToString(gender) == string,
        orElse: () => Gender.unknown);
  }

  String _dateToText(DateTime dateTime) {
    return dateTime == null
        ? "Not provided"
        : "${dateTime.toLocal()}".split(' ')[0];
  }

  Future<void> _selectBirthDate(BuildContext context) async {
    final DateTime picked = await showDatePicker(
      context: context,
      initialDate: person.birthDate ?? DateTime.now(),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
      initialEntryMode: DatePickerEntryMode.input,
      helpText: 'Select family members birthdate',
    );

    if (picked != null) {
      setState(() => person.birthDate = picked);
    }
  }

  Future<void> _selectDeathDate(BuildContext context) async {
    final DateTime picked = await showDatePicker(
      context: context,
      initialDate: person.deathDate ?? DateTime.now(),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
      initialEntryMode: DatePickerEntryMode.input,
      helpText: 'Select family members death date',
    );

    if (picked != null) {
      setState(() => person.deathDate = picked);
    }
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    // ignore: close_sinks
    final bloc = BlocProvider.of<TreeBloc>(context);

    return SingleChildScrollView(
      child: Align(
        alignment: const Alignment(0, -1 / 3),
        child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Form(
              key: formKey,
              child: Column(children: [
                Text(
                  "Provide as much information about the family member as you want, you can always come back later here ☺",
                  style: theme.textTheme.headline5,
                  textAlign: TextAlign.center,
                ),
                const SizedBox(height: 8.0),
                Column(mainAxisSize: MainAxisSize.min, children: <Widget>[
                  Avatar(photo: person.mainPhoto?.uri, avatarSize: 48),
                  const SizedBox(height: 4.0),
                  TextFormField(
                    initialValue: person.name,
                    onChanged: (firstname) =>
                        setState(() => person.name = firstname),
                    decoration: const InputDecoration(
                      labelText: 'firstname',
                    ),
                  ),
                  const SizedBox(height: 8.0),
                  TextFormField(
                    initialValue: person.lastName,
                    onChanged: (lastName) =>
                        setState(() => person.lastName = lastName),
                    decoration: const InputDecoration(
                      labelText: 'lastname',
                    ),
                  ),
                  const SizedBox(height: 8.0),
                  Text('Gender:'),
                  const SizedBox(height: 4.0),
                  Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 8.0),
                      child: Column(
                          children: Gender.values
                              .map((gender) => RadioListTile<Gender>(
                                    title: Text(_mapGenderToString(gender)),
                                    value: gender,
                                    groupValue:
                                        _mapStringToGender(person.gender),
                                    onChanged: (gender) => setState(() => person
                                        .gender = _mapGenderToString(gender)),
                                  ))
                              .toList())),
                  const SizedBox(height: 8.0),
                  Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: [
                        Text(
                          'birthdate:',
                          style: theme.textTheme.bodyText1,
                        ),
                        const SizedBox(width: 20.0),
                        RaisedButton(
                            onPressed: () => _selectBirthDate(context),
                            child: Text(_dateToText(person.birthDate))),
                      ]),
                  const SizedBox(height: 8.0),
                  Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: [
                        Text(
                          'death date:',
                          style: theme.textTheme.bodyText1,
                        ),
                        const SizedBox(width: 20.0),
                        RaisedButton(
                            onPressed: () => _selectDeathDate(context),
                            child: Text(_dateToText(person.deathDate))),
                      ]),
                  const SizedBox(height: 8.0),
                  TextFormField(
                    initialValue: person.description,
                    onChanged: (description) =>
                        setState(() => person.description = description),
                    decoration: const InputDecoration(
                      labelText: 'description',
                    ),
                  ),
                  const SizedBox(height: 8.0),
                  TextFormField(
                    initialValue: person.biography,
                    onChanged: (biography) =>
                        setState(() => person.biography = biography),
                    decoration: const InputDecoration(
                      labelText: 'biography',
                    ),
                  ),
                  const SizedBox(height: 8.0),
                  RaisedButton(
                      child: Text(adding ? 'ADD' : 'SAVE'),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(30.0),
                      ),
                      color: theme.accentColor,
                      disabledColor: theme.disabledColor,
                      onPressed: () {
                        if (adding) {
                          bloc.add(PersonAdded(person));
                        }

                        Navigator.of(context).pop();
                      }),
                ]),
              ]),
            )),
      ),
    );
  }
}

enum Gender { unknown, male, female, other }
