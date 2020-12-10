import 'package:meta/meta.dart';
import 'package:tree_of_a_kind/contracts/common/base_repository.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_client.dart';

import 'contracts.dart';

class TreeRepository extends BaseRepository {
  TreeRepository(CqrsClient cqrs) : super(cqrs);

  Future<BaseQueryResult<List<TreeItemDTO>>> getMyTrees() async {
    final result = await get(GetMyTrees());

    return BaseQueryResult(result.data.trees,
        unexpectedError: result.unexpectedError || result.data == null);
  }

  Future<BaseCommandResult> addTree({@required String treeName}) {
    return run(CreateTree()..treeName = treeName);
  }
}
