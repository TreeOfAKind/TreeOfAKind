import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:progress_indicators/progress_indicators.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/features/person_files/bloc/person_files_bloc.dart';

class PersonFilesView extends StatelessWidget {
  PersonFilesView(
      {@required this.files, this.isRefreshing = false, this.deletedFileId});

  final List<FileDTO> files;
  final bool isRefreshing;
  final String deletedFileId;

  @override
  Widget build(BuildContext context) {
    return ListView(children: [
      ...files.map((item) => item.id != deletedFileId
          ? _FileItem(fileItem: item)
          : _FileItemLoading()),
      if (isRefreshing && deletedFileId == null) _FileItemLoading()
    ]);
  }
}

class _FileItem extends StatelessWidget {
  _FileItem({@required this.fileItem});

  final FileDTO fileItem;

  void _deleteFileDialog(BuildContext context, PersonFilesBloc bloc) {
    final theme = Theme.of(context);

    showDialog(
        context: context,
        builder: (context) => new AlertDialog(
              title: Text('Are you sure?'),
              titleTextStyle: theme.textTheme.headline4,
              content: Text('Do you want to delete file ${fileItem.name}?',
                  style: theme.textTheme.bodyText1),
              actions: <Widget>[
                TextButton(
                  child: Text('No'),
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                ),
                TextButton(
                  child: Text('Yes'),
                  onPressed: () {
                    Navigator.of(context).pop();
                    bloc.add(FileDeleted(fileItem.id));
                  },
                ),
              ],
            ));
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Slidable(
        actionPane: SlidableDrawerActionPane(),
        child: Card(
            shadowColor: theme.shadowColor,
            child: ListTile(
              leading: Icon(Icons.folder
                  //color: theme.accentColor,
                  ),
              title: Text(fileItem.name, style: theme.textTheme.headline6),
              trailing: Icon(Icons.cloud_download),
            )),
        secondaryActions: [
          IconSlideAction(
            caption: 'Delete',
            color: theme.errorColor,
            icon: Icons.delete,
            onTap: () => _deleteFileDialog(
                context, BlocProvider.of<PersonFilesBloc>(context)),
          ),
        ]);
  }
}

class _FileItemLoading extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Card(
        shadowColor: theme.shadowColor,
        child: ListTile(
          leading: Icon(Icons.folder_special
              //color: theme.accentColor,
              ),
          title: JumpingDotsProgressIndicator(
            fontSize: theme.textTheme.headline6.fontSize,
          ),
        ));
  }
}
