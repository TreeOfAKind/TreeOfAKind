using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using TreeOfAKind.Application.Configuration.Authorization;
using TreeOfAKind.Application.Configuration.Data;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees
{
    public class TreeOperationCommandAuthorizer<TResponse> : IAuthorizer<TreeOperationCommandBase<TResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public TreeOperationCommandAuthorizer(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(TreeOperationCommandBase<TResponse> instance, CancellationToken cancellation = default)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            const string sql =
                "SELECT 1 FROM [trees].[TreeUserProfile] tup " +
                "JOIN [trees].UserProfiles up ON up.[Id] = tup.[UserId] " +
                "WHERE tup.[TreeId] = @ownedTree AND up.[UserAuthId] = @treeOwner";
            var owns = await connection.QueryFirstOrDefaultAsync<int?>(sql,
                new {ownedTree = instance.TreeId.Value, treeOwner = instance.RequesterUserAuthId});

            return owns.HasValue
                ? AuthorizationResult.Succeed()
                : AuthorizationResult.Fail("User is not owner of this tree");
        }
    }
}
