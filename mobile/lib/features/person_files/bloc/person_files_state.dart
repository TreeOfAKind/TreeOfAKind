part of 'person_files_bloc.dart';

abstract class PersonFilesState {
  const PersonFilesState();
}

class PresentingFiles extends PersonFilesState {
  const PresentingFiles(this.files);

  final List<FileDTO> files;
}

class LoadingState extends PersonFilesState {
  const LoadingState(this.fileId);

  final String fileId;
}

class UnknownErrorState extends PersonFilesState {
  const UnknownErrorState();
}
