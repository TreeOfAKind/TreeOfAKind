import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:progress_indicators/progress_indicators.dart';
import 'package:tree_of_a_kind/contracts/user_profile/contracts.dart';
import 'package:tree_of_a_kind/features/owners/bloc/owners_bloc.dart';
import 'package:tree_of_a_kind/features/tree/bloc/tree_bloc.dart';

class OwnersView extends StatelessWidget {
  OwnersView(
      {@required this.owners, this.isRefreshing = false, this.deletedOwnerId});

  final List<UserProfileDTO> owners;
  final bool isRefreshing;
  final String deletedOwnerId;

  @override
  Widget build(BuildContext context) {
    return ListView(children: [
      ...owners.map((item) => item.id != deletedOwnerId
          ? _OwnerItem(ownerItem: item)
          : _OwnerItemLoading()),
      if (isRefreshing && deletedOwnerId == null) _OwnerItemLoading()
    ]);
  }
}

class _OwnerItem extends StatelessWidget {
  _OwnerItem({@required this.ownerItem});

  final UserProfileDTO ownerItem;

  String _getName(UserProfileDTO owner) =>
      owner.firstName.isNotEmpty && owner.lastName.isNotEmpty
          ? '${owner.firstName} ${owner.lastName}'
          : owner.lastName.isNotEmpty
              ? owner.lastName
              : owner.firstName.isNotEmpty
                  ? owner.firstName
                  : '<Name not provided>';

  void _deleteOwnerDialog(BuildContext context, OwnersBloc bloc) {
    final theme = Theme.of(context);

    showDialog(
        context: context,
        builder: (context) => new AlertDialog(
              title: Text('Are you sure?'),
              titleTextStyle: theme.textTheme.headline4,
              content: Text(
                  'Do you want to revoke access to the tree for user ${_getName(ownerItem)}?',
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
                    bloc.add(RemoveOwner(ownerItem.id));
                    Navigator.of(context).pop();
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
              leading: Icon(
                Icons.person,
                color: theme.accentColor,
              ),
              title:
                  Text(_getName(ownerItem), style: theme.textTheme.headline6),
            )),
        secondaryActions: [
          IconSlideAction(
            caption: 'Delete',
            color: theme.errorColor,
            icon: Icons.person_remove,
            onTap: () => _deleteOwnerDialog(
                context, BlocProvider.of<OwnersBloc>(context)),
          ),
        ]);
  }
}

class _OwnerItemLoading extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Card(
        shadowColor: theme.shadowColor,
        child: ListTile(
          leading: Icon(
            Icons.nature,
            color: theme.accentColor,
          ),
          title: JumpingDotsProgressIndicator(
            fontSize: theme.textTheme.headline6.fontSize,
          ),
        ));
  }
}
