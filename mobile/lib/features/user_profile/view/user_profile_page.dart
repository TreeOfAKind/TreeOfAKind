import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:tree_of_a_kind/contracts/user_profile/user_profile_repository.dart';
import 'package:tree_of_a_kind/features/authentication/authentication.dart';
import 'package:tree_of_a_kind/features/common/generic_error.dart';
import 'package:tree_of_a_kind/features/common/loading_indicator.dart';
import 'package:tree_of_a_kind/features/user_profile/bloc/user_profile_bloc.dart';
import 'package:tree_of_a_kind/features/user_profile/view/user_profile_view.dart';

class UserProfilePage extends StatefulWidget {
  @override
  _UserProfilePageState createState() => _UserProfilePageState();

  static Route route() {
    return MaterialPageRoute<void>(
        builder: (context) => BlocProvider<UserProfileBloc>(
              create: (context) => UserProfileBloc(
                  userProfileRepository:
                      RepositoryProvider.of<UserProfileRepository>(context))
                ..add(const FetchUserProfile()),
              child: UserProfilePage(),
            ));
  }
}

class _UserProfilePageState extends State<UserProfilePage> {
  @override
  Widget build(BuildContext context) {
    final user = BlocProvider.of<AuthenticationBloc>(context).state.user;

    return BlocBuilder<UserProfileBloc, UserProfileState>(
      builder: (context, state) {
        if (state is LoadingState) {
          return LoadingIndicator();
        } else if (state is UnknownErrorState) {
          return GenericError();
        } else if (state is PresentingData) {
          return UserProfileView(
              user: user,
              userProfile: state.userProfile,
              canSave: state.hasChanged);
        } else {
          return GenericError();
        }
      },
    );
  }
}
