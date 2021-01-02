import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/people/people_repository.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/features/people/bloc/people_bloc.dart';
import 'package:tree_of_a_kind/features/people/view/add_or_update_person_view.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class AddPersonPage extends StatelessWidget {
  const AddPersonPage({Key key, @required this.tree}) : super(key: key);

  final TreeDTO tree;

  static Route route(TreeBloc bloc, TreeDTO tree) {
    return MaterialPageRoute<void>(
      builder: (context) => BlocProvider<PeopleBloc>(
        create: (context) => PeopleBloc(
            treeBloc: bloc,
            peopleRepository: RepositoryProvider.of<PeopleRepository>(context)),
        child: AddPersonPage(tree: tree),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: Text('Add a new member to your family')),
        body: AddOrUpdatePersonView(tree: tree));
  }
}
