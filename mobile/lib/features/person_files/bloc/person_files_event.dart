part of 'person_files_bloc.dart';

abstract class PersonFilesEvent {
  const PersonFilesEvent();
}

class FileAdded extends PersonFilesEvent {
  const FileAdded(this.file);

  final PlatformFile file;
}

class FileDeleted extends PersonFilesEvent {
  const FileDeleted(this.fileId);

  final String fileId;
}
