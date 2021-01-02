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
  String _getFullname(PersonDTO person) => '${person.name} ${person.lastName}';

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

  String _getInitialValue() {
    final personId = _getRelatedPersonId();

    return personId == null
        ? null
        : _getFullname(
            widget.otherPeople.firstWhere((person) => person.id == personId));
  }

  @override
  Widget build(BuildContext context) {
    return TypeAheadFormField<PersonDTO>(
        initialValue: _getInitialValue(),
        textFieldConfiguration: TextFieldConfiguration(
            decoration: new InputDecoration(
                labelText: EnumToString.convertToString(widget.relation,
                    camelCase: true),
                helperText:
                    'Select ${EnumToString.convertToString(widget.relation)} of this family member',
                suffixIcon: const Icon(Icons.delete))),
        onSuggestionSelected: (person) => setState(() {
              switch (widget.relation) {
                case Relation.mother:
                  widget.relatingPerson.mother = person.id;
                  break;
                case Relation.father:
                  widget.relatingPerson.father = person.id;
                  break;
                case Relation.spouse:
                  widget.relatingPerson.spouse = person.id;
                  break;
              }
            }),
        itemBuilder: (context, person) =>
            ListTile(title: Text(_getFullname(person))),
        suggestionsCallback: (query) => widget.otherPeople.where((person) =>
            widget.relatingPerson.id != person.id &&
            _getFullname(person).toLowerCase().contains(query.toLowerCase())));
  }
}

enum Relation { mother, father, spouse }
