import 'package:enum_to_string/enum_to_string.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/common/avatar.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/common/loading_indicator.dart';
import 'package:tree_of_a_kind/features/people/bloc/people_bloc.dart';
import 'package:tree_of_a_kind/features/people/view/auto_complete_relation.dart';
import 'package:recase/recase.dart';

class AddOrUpdatePersonView extends StatefulWidget {
  AddOrUpdatePersonView({@required this.tree, PersonDTO person})
      : adding = person == null,
        person = person ?? PersonDTO()
          ..gender = mapGenderToString(Gender.unknown);

  final TreeDTO tree;
  final PersonDTO person;
  final bool adding;

  static String mapGenderToString(Gender gender) {
    return EnumToString.convertToString(gender, camelCase: true);
  }

  static Gender mapStringToGender(String string) {
    return Gender.values.firstWhere(
        (gender) =>
            mapGenderToString(gender).toLowerCase() == string?.toLowerCase(),
        orElse: () => Gender.unknown);
  }

  @override
  _AddOrUpdatePersonViewState createState() => _AddOrUpdatePersonViewState();
}

class _AddOrUpdatePersonViewState extends State<AddOrUpdatePersonView> {
  final formKey = GlobalKey<FormState>();

  PlatformFile _selectedMainPhoto;
  bool _photoDeleted;

  String _dateToText(DateTime dateTime) {
    return dateTime == null
        ? "Not provided"
        : "${dateTime.toLocal()}".split(' ')[0];
  }

  Future<void> _selectBirthDate(BuildContext context) async {
    final DateTime picked = await showDatePicker(
      context: context,
      initialDate: widget.person.birthDate ?? DateTime.now(),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
      initialEntryMode: DatePickerEntryMode.calendar,
      helpText: 'Select family members birthdate',
    );

    if (picked != null) {
      setState(() => widget.person.birthDate = picked);
    }
  }

  Future<void> _selectDeathDate(BuildContext context) async {
    final DateTime picked = await showDatePicker(
      context: context,
      initialDate: widget.person.deathDate ?? DateTime.now(),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
      initialEntryMode: DatePickerEntryMode.calendar,
      helpText: 'Select family members death date',
    );

    if (picked != null) {
      setState(() => widget.person.deathDate = picked);
    }
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    // ignore: close_sinks
    final bloc = BlocProvider.of<PeopleBloc>(context);

    return BlocBuilder<PeopleBloc, PeopleState>(builder: (context, state) {
      var saving = false;

      if (state is PersonSavedSuccessfully) {
        WidgetsBinding.instance
            .addPostFrameCallback((_) => Navigator.of(context).pop());
      } else if (state is UnknownErrorState) {
        return GenericError();
      } else if (state is ValidationErrorState) {
        WidgetsBinding.instance.addPostFrameCallback((_) async => showDialog(
            context: context,
            builder: (context) => AlertDialog(
                    title: Text(
                        '${widget.adding ? "Adding" : "Saving"} family member failed'),
                    content: Text(state.errorText),
                    actions: [
                      TextButton(
                          child: Text('OK... 😔'),
                          onPressed: () {
                            Navigator.of(context).pop();
                          })
                    ])));
      } else if (state is SavingState) {
        saving = true;
      }

      return SingleChildScrollView(
        child: Align(
          alignment: const Alignment(0, -1 / 3),
          child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Form(
                key: formKey,
                child: Column(children: [
                  Text(
                    "Provide as much information as you want, you can always come back to edit this family member ☺",
                    style: theme.textTheme.headline5,
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 8.0),
                  Column(mainAxisSize: MainAxisSize.min, children: <Widget>[
                    Avatar(photo: widget.person.mainPhoto?.uri, avatarSize: 48),
                    const SizedBox(height: 16.0),
                    TextFormField(
                      initialValue: widget.person.name,
                      onChanged: (firstname) =>
                          setState(() => widget.person.name = firstname),
                      validator: (text) => text.isEmpty
                          ? 'Please provide their firstname'
                          : null,
                      decoration: const InputDecoration(
                        labelText: 'firstname',
                      ),
                    ),
                    const SizedBox(height: 8.0),
                    TextFormField(
                      initialValue: widget.person.lastName,
                      onChanged: (lastName) =>
                          setState(() => widget.person.lastName = lastName),
                      validator: (text) =>
                          text.isEmpty ? 'Please provide their lastname' : null,
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
                                      title: Text(AddOrUpdatePersonView
                                          .mapGenderToString(gender)),
                                      value: gender,
                                      groupValue: AddOrUpdatePersonView
                                          .mapStringToGender(
                                              widget.person.gender),
                                      onChanged: (gender) => setState(() =>
                                          widget.person.gender =
                                              AddOrUpdatePersonView
                                                  .mapGenderToString(gender)),
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
                              onLongPress: () => setState(
                                  () => widget.person.birthDate = null),
                              child:
                                  Text(_dateToText(widget.person.birthDate))),
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
                              onLongPress: () => setState(
                                  () => widget.person.deathDate = null),
                              child:
                                  Text(_dateToText(widget.person.deathDate))),
                        ]),
                    const SizedBox(height: 8.0),
                    TextFormField(
                      initialValue: widget.person.description,
                      onChanged: (description) => setState(
                          () => widget.person.description = description),
                      decoration: const InputDecoration(
                        labelText: 'description',
                      ),
                    ),
                    const SizedBox(height: 8.0),
                    TextFormField(
                      initialValue: widget.person.biography,
                      onChanged: (biography) =>
                          setState(() => widget.person.biography = biography),
                      decoration: const InputDecoration(
                        labelText: 'biography',
                      ),
                    ),
                    const SizedBox(height: 16.0),
                    Text('Family member relatives:'),
                    ...Relation.values
                        .expand((relation) => [
                              const SizedBox(height: 8.0),
                              AutoCompleteRelation(
                                relation: relation,
                                relatingPerson: widget.person,
                                otherPeople: widget.tree.people,
                              )
                            ])
                        .toList(),
                    const SizedBox(height: 12.0),
                    saving
                        ? LoadingIndicator()
                        : RaisedButton(
                            child: Text(widget.adding ? 'ADD' : 'SAVE'),
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(30.0),
                            ),
                            color: theme.accentColor,
                            onPressed: () {
                              if (formKey.currentState.validate()) {
                                bloc.add(widget.adding
                                    ? PersonAdded(
                                        widget.person, _selectedMainPhoto)
                                    : PersonUpdated(widget.person,
                                        _selectedMainPhoto, _photoDeleted));
                              }
                            }),
                  ]),
                ]),
              )),
        ),
      );
    });
  }
}

enum Gender { unknown, male, female, other }
