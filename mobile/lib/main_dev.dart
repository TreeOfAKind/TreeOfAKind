import 'package:tree_of_a_kind/app/config.dart';
import 'package:tree_of_a_kind/main_common.dart';

void main() {
  final config = Config()
    ..apiUri = Uri.parse("https://treeofakind-test.azurewebsites.net/api/");

  mainCommon(config);
}
