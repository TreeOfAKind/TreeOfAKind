import 'package:file_picker/file_picker.dart';
import 'package:meta/meta.dart';
import 'package:tree_of_a_kind/contracts/common/base_repository.dart';
import 'package:tree_of_a_kind/contracts/owners/contracts.dart';
import 'package:tree_of_a_kind/features/cqrs/cqrs_client.dart';

import 'contracts.dart';

class TreeRepository extends BaseRepository {
  TreeRepository(CqrsClient cqrs) : super(cqrs);

  Future<BaseQueryResult<List<TreeItemDTO>>> getMyTrees() async {
    final result = await get(GetMyTrees());

    return BaseQueryResult(result.data.trees,
        unexpectedError: result.unexpectedError || result.data == null);
  }

  Future<BaseQueryResult<TreeDTO>> getTreeDetails(
      {@required String treeId}) async {
    final result = await get(GetTree(treeId: treeId));

    return BaseQueryResult(result.data,
        unexpectedError: result.unexpectedError || result.data == null);
  }

  Future<BaseCommandResult> addTree({@required String treeName}) {
    return run(CreateTree()..treeName = treeName);
  }

  Future<BaseCommandResult> updateTreePhoto(
      {@required String treeId, PlatformFile image}) {
    return image != null
        ? runWithFile(AddOrChangeTreePhoto(treeId: treeId, image: image))
        : run(RemoveTreePhoto(treeId: treeId));
  }

  Future<BaseCommandResult> deleteTree({@required String treeId}) {
    return run(RemoveTreeOwner()..treeId = treeId);
  }
}
