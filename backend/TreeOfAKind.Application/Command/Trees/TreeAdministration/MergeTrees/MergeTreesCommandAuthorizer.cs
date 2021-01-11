using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using TreeOfAKind.Application.Configuration.Authorization;
using TreeOfAKind.Application.Configuration.Data;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.MergeTrees
{
    public class MergeTreesCommandAuthorizer : IAuthorizer<MergeTreesCommand>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public MergeTreesCommandAuthorizer(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(MergeTreesCommand instance,
            CancellationToken cancellation = default)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            const string sql =
                "SELECT TOP 2 1 FROM [trees].[TreeUserProfile] tup " +
                "JOIN [trees].UserProfiles up ON up.[Id] = tup.[UserId] " +
                "WHERE (tup.[TreeId] = @firstTree OR tup.[TreeId] = @secondTree) AND up.[UserAuthId] = @treeOwner";
            var ownsFirst = await connection.QueryAsync<int>(sql,
                new {firstTree = instance.First.Value, secondTree = instance.Second.Value, treeOwner = instance.RequesterUserAuthId});


            return ownsFirst?.Count() > 1
                ? AuthorizationResult.Succeed()
                : AuthorizationResult.Fail("User is not owner of this tree");
        }
    }
}
