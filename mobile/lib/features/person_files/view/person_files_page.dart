import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/features/common/empty_widget_info.dart';
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

  Future<void> _pickFile(BuildContext context) async {
    final result = await FilePicker.platform.pickFiles(
      type: FileType.custom,
      allowedExtensions: ['jpg', 'jpeg', 'png', 'pdf'],
    );

    final file = result?.files?.single;

    if (file != null) {
      BlocProvider.of<PersonFilesBloc>(context).add(FileAdded(file));
    }
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
        appBar: AppBar(title: Text('${_getFullname(widget.person)} files')),
        floatingActionButton: FloatingActionButton(
            child: Icon(Icons.upload_file),
            onPressed: () => _pickFile(context)),
        body: BlocBuilder<PersonFilesBloc, PersonFilesState>(
          builder: (context, state) {
            if (state is UnknownErrorState) {
              return GenericError();
            } else if (state is PresentingFiles) {
              widget.person.files = state.files;
              return widget.person.files.isEmpty
                  ? const EmptyWidgetInfo(
                      "Family member related files will be here, if you'd wish to add any ☺")
                  : PersonFilesView(files: widget.person.files);
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
