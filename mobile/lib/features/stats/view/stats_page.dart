import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/common/loading_indicator.dart';
import 'package:tree_of_a_kind/features/stats/bloc/stats_bloc.dart';
import 'package:tree_of_a_kind/features/stats/view/stats_view.dart';

class StatsPage extends StatefulWidget {
  const StatsPage({Key key, @required this.tree}) : super(key: key);

  final TreeDTO tree;

  @override
  _StatsPageState createState() => _StatsPageState();

  static Route route(TreeDTO tree) {
    return MaterialPageRoute<void>(
      builder: (context) => BlocProvider<StatsBloc>(
        create: (context) => StatsBloc(
            treeRepository: RepositoryProvider.of<TreeRepository>(context))
          ..add(FetchStats(tree.treeId)),
        child: StatsPage(tree: tree),
      ),
    );
  }
}

class _StatsPageState extends State<StatsPage> {
  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
        appBar: AppBar(title: Text("Family tree stats")),
        body: BlocBuilder<StatsBloc, StatsState>(builder: (context, state) {
          if (state is LoadingState) {
            return Center(child: LoadingIndicator());
          } else if (state is UnknownErrorState) {
            return GenericError();
          } else if (state is PresentingStats) {
            return StatsView();
          } else {
            return Container();
          }
        }));
  }
}
