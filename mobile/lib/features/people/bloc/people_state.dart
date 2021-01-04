part of 'people_bloc.dart';

abstract class PeopleState {
  const PeopleState();
}

class PresentingForm extends PeopleState {
  const PresentingForm();
}

class SavingState extends PeopleState {
  const SavingState();
}

class PersonSavedSuccessfully extends PeopleState {
  const PersonSavedSuccessfully();
}

class ValidationErrorState extends PeopleState {
  const ValidationErrorState(this.errorText);

  final String errorText;
}

class UnknownErrorState extends PeopleState {
  const UnknownErrorState();
}
