import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/tree/contracts.dart';
import 'package:tree_of_a_kind/contracts/tree/tree_repository.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/contracts/user_profile/user_profile_repository.dart';
import 'package:tree_of_a_kind/features/common/empty_widget_info.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/common/loading_indicator.dart';
import 'package:tree_of_a_kind/features/owners/bloc/owners_bloc.dart';
import 'package:tree_of_a_kind/features/owners/view/add_owner_dialog.dart';
import 'package:tree_of_a_kind/features/owners/view/owners_view.dart';

class OwnersPage extends StatefulWidget {
  OwnersPage({Key key, @required this.tree}) : super(key: key);

  final TreeDTO tree;

  @override
  _OwnersPageState createState() => _OwnersPageState();

  static Route route(TreeDTO tree) {
    return MaterialPageRoute<void>(
        builder: (context) => BlocProvider<OwnersBloc>(
            create: (context) => OwnersBloc(
                tree: tree,
                userProfileRepository:
                    RepositoryProvider.of<UserProfileRepository>(context),
                treeRepository: RepositoryProvider.of<TreeRepository>(context))
              ..add(const FetchMyProfile()),
            child: OwnersPage(tree: tree)));
  }
}

class _OwnersPageState extends State<OwnersPage> {
  List<UserProfileDTO> owners;

  Future<void> _addOwner(BuildContext context) async {
    return showDialog(
        context: context,
        builder: (_) => AddOwnerDialog(BlocProvider.of<OwnersBloc>(context)));
  }

  Widget _renderView() => owners.isEmpty
      ? EmptyWidgetInfo(
          "Other ${widget.tree.treeName} family tree owners will be here, if you'd wish to share your tree with other users ☺")
      : OwnersView(owners: owners);

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
        appBar: AppBar(
            title: Text('${widget.tree.treeName} owners'),
            leading: IconButton(
              icon: Icon(Icons.keyboard_arrow_left),
              onPressed: () => Navigator.of(context).pop(owners),
            )),
        floatingActionButton: FloatingActionButton(
            child: Icon(Icons.person_add), onPressed: () => _addOwner(context)),
        body: BlocBuilder<OwnersBloc, OwnersState>(
          builder: (context, state) {
            if (state is UnknownErrorState) {
              return GenericError();
            } else if (state is ValidationErrorState) {
              WidgetsBinding.instance.addPostFrameCallback((_) => showDialog(
                  context: context,
                  builder: (context) => AlertDialog(
                          title: Text('Adding new family tree owner failed'),
                          content: Text(state.errorText),
                          actions: [
                            TextButton(
                                child: Text('OK... 😔'),
                                onPressed: () {
                                  Navigator.of(context).pop();
                                })
                          ])));
              return _renderView();
            } else if (state is PresentingOwners) {
              owners = state.owners;
              return _renderView();
            } else if (state is LoadingState) {
              return owners == null
                  ? Center(child: LoadingIndicator())
                  : OwnersView(
                      owners: owners,
                      isRefreshing: true,
                      deletedOwnerId: state.deletedUserId);
            } else {
              return Container();
            }
          },
        ));
  }
}
