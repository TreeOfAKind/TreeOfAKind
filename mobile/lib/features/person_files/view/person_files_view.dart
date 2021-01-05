import 'dart:io';
import 'dart:isolate';
import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_downloader/flutter_downloader.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:mime/mime.dart';
import 'package:path_provider/path_provider.dart';
import 'package:permission_handler/permission_handler.dart';
import 'package:progress_indicators/progress_indicators.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/features/person_files/bloc/person_files_bloc.dart';

class PersonFilesView extends StatefulWidget {
  PersonFilesView(
      {@required this.files, this.isRefreshing = false, this.deletedFileId});

  final List<FileDTO> files;
  final bool isRefreshing;
  final String deletedFileId;

  @override
  _PersonFilesViewState createState() => _PersonFilesViewState();
}

class _PersonFilesViewState extends State<PersonFilesView> {
  ReceivePort _port = ReceivePort();
  List<_TaskInfo> _tasks;

  static void downloadCallback(
      String id, DownloadTaskStatus status, int progress) {
    final SendPort send =
        IsolateNameServer.lookupPortByName('downloader_send_port');
    send.send([id, status, progress]);
  }

  void _unbindBackgroundIsolate() {
    IsolateNameServer.removePortNameMapping('downloader_send_port');
  }

  void _bindBackgroundIsolate() {
    bool isSuccess = IsolateNameServer.registerPortWithName(
        _port.sendPort, 'downloader_send_port');
    if (!isSuccess) {
      _unbindBackgroundIsolate();
      _bindBackgroundIsolate();
      return;
    }
    _port.listen((dynamic data) {
      String id = data[0];
      DownloadTaskStatus status = data[1];
      int progress = data[2];

      if (_tasks != null && _tasks.isNotEmpty) {
        final task = _tasks.firstWhere((task) => task.taskId == id);
        if (task != null) {
          setState(() {
            task.status = status;
            task.progress = progress;
          });
        }
      }
    });
  }

  @override
  void initState() {
    super.initState();
    _bindBackgroundIsolate();
    FlutterDownloader.registerCallback(downloadCallback);
  }

  @override
  void dispose() {
    _unbindBackgroundIsolate();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return ListView(children: [
      ...widget.files.map((item) => item.id != widget.deletedFileId
          ? _FileItem(fileItem: item)
          : _FileItemLoading()),
      if (widget.isRefreshing && widget.deletedFileId == null)
        _FileItemLoading()
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

  Future<bool> _checkPermission() async {
    if (Platform.isAndroid) {
      final status = await Permission.storage.status;
      if (status != PermissionStatus.granted) {
        final result = await Permission.storage.request();
        if (result == PermissionStatus.granted) {
          return true;
        }
      } else {
        return true;
      }
    }

    return false;
  }

  Future<void> _downloadFile(BuildContext context, FileDTO file) async {
    if (await _checkPermission()) {
      final directory = Platform.isAndroid
          ? await getExternalStorageDirectory()
          : await getApplicationDocumentsDirectory();

      var downloadDirectory =
          Directory(directory.path + Platform.pathSeparator + 'Download');

      if (!await downloadDirectory.exists()) {
        downloadDirectory = await downloadDirectory.create();
      }

      await FlutterDownloader.enqueue(
        savedDir: downloadDirectory.path,
        url: file.uri,
        fileName: file.name,
      );
    } else {
      Scaffold.of(context).showSnackBar(SnackBar(
          content: Text(
              "Couldn't download the file, because no permission was granted."),
          action: SnackBarAction(
              label: 'Try again',
              onPressed: () => _downloadFile(context, file))));
    }
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Slidable(
        actionPane: SlidableDrawerActionPane(),
        child: Card(
            shadowColor: theme.shadowColor,
            child: ListTile(
              leading: lookupMimeType(fileItem.name) == "application/pdf"
                  ? Icon(Icons.file_present)
                  : Icon(Icons.image),
              title: Text(fileItem.name, style: theme.textTheme.headline6),
              trailing: IconButton(
                  icon: Icon(Icons.cloud_download),
                  onPressed: () async =>
                      await _downloadFile(context, fileItem)),
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
          leading: Icon(Icons.file_download),
          title: JumpingDotsProgressIndicator(
            fontSize: theme.textTheme.headline6.fontSize,
          ),
        ));
  }
}

class _TaskInfo {
  final String name;
  final String link;

  String taskId;
  int progress = 0;
  DownloadTaskStatus status = DownloadTaskStatus.undefined;

  _TaskInfo({this.name, this.link});
}
