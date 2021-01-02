import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:flutter_typeahead/flutter_typeahead.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:recase/recase.dart';

class AutoCompleteRelation extends StatefulWidget {
  const AutoCompleteRelation(
      {Key key, this.relation, this.relatingPerson, this.otherPeople})
      : super(key: key);

  final Relation relation;
  final PersonDTO relatingPerson;
  final List<PersonDTO> otherPeople;

  @override
  _AutoCompleteRelationState createState() => _AutoCompleteRelationState();
}

class _AutoCompleteRelationState extends State<AutoCompleteRelation> {
  final TextEditingController textController = TextEditingController();

  String _getFullname(PersonDTO person) =>
      person == null ? null : '${person.name} ${person.lastName}';

  // ignore: missing_return
  String _getRelatedPersonId() {
    switch (widget.relation) {
      case Relation.mother:
        return widget.relatingPerson.mother;
        break;
      case Relation.father:
        return widget.relatingPerson.father;
        break;
      case Relation.spouse:
        return widget.relatingPerson.spouse;
        break;
    }
  }

  void _setRelatedPersonId({String id}) {
    switch (widget.relation) {
      case Relation.mother:
        widget.relatingPerson.mother = id;
        break;
      case Relation.father:
        widget.relatingPerson.father = id;
        break;
      case Relation.spouse:
        widget.relatingPerson.spouse = id;
        break;
    }
  }

  String _getRelatedPersonFullName() {
    final personId = _getRelatedPersonId();

    return personId == null
        ? null
        : _getFullname(
            widget.otherPeople.firstWhere((person) => person.id == personId));
  }

  Iterable<PersonDTO> _getPossiblePeople() => widget.otherPeople
      .where((person) => _getFullname(person) == textController.text);

  @override
  void initState() {
    super.initState();
    textController.text = _getRelatedPersonFullName();
    textController.addListener(() {
      final possiblePeople = _getPossiblePeople();

      _setRelatedPersonId(
          id: possiblePeople.isNotEmpty ? possiblePeople.first.id : null);
    });
  }

  @override
  Widget build(BuildContext context) {
    return TypeAheadFormField<PersonDTO>(
        textFieldConfiguration: TextFieldConfiguration(
            controller: textController,
            decoration: new InputDecoration(
              labelText: EnumToString.convertToString(widget.relation,
                  camelCase: true),
              suffixIcon: FlatButton(
                  child: const Icon(Icons.clear),
                  onPressed: () => textController.clear()),
            )),
        validator: (text) => text.isNotEmpty && _getPossiblePeople().isEmpty
            ? 'Provide full name of ${EnumToString.convertToString(widget.relation)} or leave this field empty'
            : null,
        onSuggestionSelected: (person) => setState(() {
              _setRelatedPersonId(id: person.id);
              textController.text = _getRelatedPersonFullName();
            }),
        itemBuilder: (context, person) =>
            ListTile(title: Text(_getFullname(person))),
        suggestionsCallback: (query) => query?.isEmpty ?? true
            ? List()
            : widget.otherPeople
                .where((person) =>
                    widget.relatingPerson.id != person.id &&
                    _getFullname(person)
                        .toLowerCase()
                        .contains(query.toLowerCase()))
                .take(5)
                .toList()
          ..sort((person1, person2) =>
              _getFullname(person1).compareTo(_getFullname(person2))));
  }
}

enum Relation { mother, father, spouse }
