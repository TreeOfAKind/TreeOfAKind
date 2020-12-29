import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/contracts.dart';
import 'package:tree_of_a_kind/features/people/view/add_or_update_person_view.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class UpdatePersonPage extends StatefulWidget {
  const UpdatePersonPage(
      {Key key, @required this.treeId, @required this.person})
      : super(key: key);

  final String treeId;
  final PersonDTO person;

  @override
  _UpdatePersonPageState createState() =>
      _UpdatePersonPageState(treeId, person);

  static Route route(TreeBloc bloc, String treeId, PersonDTO person) {
    return MaterialPageRoute<void>(
      builder: (context) => BlocProvider<TreeBloc>(
        create: (context) => bloc,
        child: UpdatePersonPage(treeId: treeId, person: person),
      ),
    );
  }
}

class _UpdatePersonPageState extends State<UpdatePersonPage> {
  _UpdatePersonPageState(this.treeId, this.person);

  final String treeId;
  final PersonDTO person;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: Text('Family member details')),
        body: AddOrUpdatePersonView(treeId: treeId, person: person));
  }
}
