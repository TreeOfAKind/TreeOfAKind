import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/person_files/bloc/person_files_bloc.dart';
import 'package:tree_of_a_kind/features/person_files/view/person_files_view.dart';

class PersonFilesPage extends StatefulWidget {
  PersonFilesPage({Key key, @required this.person}) : super(key: key);

  final PersonDTO person;

  @override
  _PersonFilesPageState createState() => _PersonFilesPageState();

  static Route route(String treeId, PersonDTO person) {
    return MaterialPageRoute<void>(
        builder: (context) => BlocProvider<PersonFilesBloc>(
            create: (context) => PersonFilesBloc(
                treeId: treeId,
                person: person,
                peopleRepository:
                    RepositoryProvider.of<PeopleRepository>(context)),
            child: PersonFilesPage(person: person)));
  }
}

class _PersonFilesPageState extends State<PersonFilesPage> {
  String _getFullname(PersonDTO person) => '${person.name} ${person.lastName}';

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
        appBar: AppBar(title: Text('${_getFullname(widget.person)} files')),
        floatingActionButton: FloatingActionButton(
            child: Icon(Icons.upload_file), onPressed: () => {}),
        body: BlocBuilder<PersonFilesBloc, PersonFilesState>(
          builder: (context, state) {
            if (state is UnknownErrorState) {
              return GenericError();
            } else if (state is PresentingFiles) {
              widget.person.files = state.files;
              return PersonFilesView(files: widget.person.files);
            } else if (state is LoadingState) {
              return PersonFilesView(
                files: widget.person.files,
                isRefreshing: true,
                deletedFileId: state.fileId,
              );
            } else {
              return Container();
            }
          },
        ));
  }
}
