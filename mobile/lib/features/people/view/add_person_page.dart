import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/features/people/view/add_or_update_person_view.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class AddPersonPage extends StatefulWidget {
  const AddPersonPage({Key key, @required this.treeId}) : super(key: key);

  final String treeId;

  @override
  _AddPersonPageState createState() => _AddPersonPageState(treeId);

  static Route route(TreeBloc bloc, String treeId) {
    return MaterialPageRoute<void>(
      builder: (context) => BlocProvider<TreeBloc>(
        create: (context) => bloc,
        child: AddPersonPage(treeId: treeId),
      ),
    );
  }
}

class _AddPersonPageState extends State<AddPersonPage> {
  _AddPersonPageState(this.treeId);

  final String treeId;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: Text('New family member')),
        body: AddOrUpdatePersonView(treeId: treeId));
  }
}
