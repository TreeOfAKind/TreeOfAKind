import 'package:autocomplete_textfield/autocomplete_textfield.dart';
import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:recase/recase.dart';

class AutoCompleteRelation extends StatefulWidget {
  const AutoCompleteRelation(
      {Key key,
      this.relation,
      this.onSubmitted,
      this.relatingPerson,
      this.otherPeople})
      : super(key: key);

  final Relation relation;
  final dynamic Function(PersonDTO) onSubmitted;
  final PersonDTO relatingPerson;
  final List<PersonDTO> otherPeople;

  @override
  _AutoCompleteRelationState createState() => _AutoCompleteRelationState();
}

class _AutoCompleteRelationState extends State<AutoCompleteRelation> {
  String _getFullname(PersonDTO person) => '${person.name} ${person.lastName}';

  @override
  Widget build(BuildContext context) {
    return AutoCompleteTextField<PersonDTO>(
      key: GlobalKey(),
      decoration: new InputDecoration(
          labelText:
              EnumToString.convertToString(widget.relation, camelCase: true),
          helperText:
              'Select ${EnumToString.convertToString(widget.relation)} of this family member',
          suffixIcon: const Icon(Icons.delete)),
      itemBuilder: (context, person) => Text(_getFullname(person)),
      itemFilter: (person, query) =>
          _getFullname(person).toLowerCase().contains(query.toLowerCase()),
      itemSorter: (person1, person2) =>
          _getFullname(person1).compareTo(_getFullname(person2)),
      itemSubmitted: (person) => setState(() {
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

        widget.onSubmitted(person);
      }),
      suggestions: widget.otherPeople
          .where((person) => widget.relatingPerson.id != person.id)
          .toList(),
    );
  }
}

enum Relation { mother, father, spouse }
