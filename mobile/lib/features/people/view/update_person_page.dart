import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/people/bloc/people_bloc.dart';
import 'package:tree_of_a_kind/features/people/view/add_or_update_person_view.dart';
import 'package:tree_of_a_kind/features/person_files/view/person_files_page.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class UpdatePersonPage extends StatelessWidget {
  const UpdatePersonPage(
      {Key key,
      @required this.tree,
      @required this.person,
      @required this.treeBloc})
      : super(key: key);

  static const String _manageFiles = 'Manage family member files';
  static const String _deletePerson = 'Delete family member';
  static const List<String> _menuItems = <String>[_manageFiles, _deletePerson];

  final TreeDTO tree;
  final PersonDTO person;
  final TreeBloc treeBloc;

  static Route route(TreeBloc bloc, TreeDTO tree, PersonDTO person) {
    return MaterialPageRoute<void>(
      builder: (context) => BlocProvider<PeopleBloc>(
        create: (context) => PeopleBloc(
            treeBloc: bloc,
            peopleRepository: RepositoryProvider.of<PeopleRepository>(context)),
        child: UpdatePersonPage(tree: tree, person: person, treeBloc: bloc),
      ),
    );
  }

  void _menuAction(String item, BuildContext context) {
    if (item == _deletePerson) {
      treeBloc.add(PersonRemoved(person.id));
      Navigator.of(context).pop();
    } else if (item == _manageFiles) {
      Navigator.of(context).push(PersonFilesPage.route(tree.treeId, person));
    }
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
        appBar: AppBar(
          title: Text('Family member details'),
          actions: <Widget>[
            PopupMenuButton(
              icon: const Icon(Icons.more_vert),
              onSelected: (item) => _menuAction(item, context),
              itemBuilder: (context) => _menuItems
                  .map((item) => PopupMenuItem(
                      value: item,
                      child: Text(item,
                          style: TextStyle(
                              color: item != _deletePerson
                                  ? theme.textTheme.bodyText1.color
                                  : theme.errorColor))))
                  .toList(),
            )
          ],
        ),
        body: AddOrUpdatePersonView(tree: tree, person: person));
  }
}
