using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Serilog.Core;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Application.Configuration.Emails;
using TreeOfAKind.Application.Services;
using TreeOfAKind.Infrastructure;
using TreeOfAKind.Infrastructure.Emails;
using TreeOfAKind.Infrastructure.FileStorage;

namespace TreeOfAKind.IntegrationTests
{
    public class ApplicationFixture : IDisposable
    {
        public string ConnectionString { get; }
        public EmailsSettings EmailsSettings { get; }

        public AzureBlobStorageSettings AzureBlobStorageSettings { get; }
        public IEmailSender EmailSender { get; }

        public IFileSaver FileSaver { get; }
        public IExecutionContextAccessor ExecutionContext { get; }
        public IUserAuthIdProvider UserAuthIdProvider { get; }

        public ApplicationFixture()
        {
            const string connectionStringEnvironmentVariable =
                "ASPNETCORE_TreeOfAKind_IntegrationTests_ConnectionString";
            ConnectionString = Environment.GetEnvironmentVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to integration tests database using environment variable: {connectionStringEnvironmentVariable}");
            }

            using var sqlConnection = new SqlConnection(ConnectionString);

            ClearDatabase(sqlConnection);

            EmailsSettings = new EmailsSettings {FromAddressEmail = "from@mail.com"};

            AzureBlobStorageSettings = new AzureBlobStorageSettings
            {
                ConnectionString = "someConnectionString",
                Metadata = new Dictionary<string, string>
                {
                    {"IntegrationTesting", "true"}
                }
            };

            EmailSender = Substitute.For<IEmailSender>();

            FileSaver = Substitute.For<IFileSaver>();

            UserAuthIdProvider = Substitute.For<IUserAuthIdProvider>();

            ExecutionContext = new ExecutionContextMock();

            ApplicationStartup.Initialize(
                new ServiceCollection(),
                ConnectionString,
                new CacheStore(),
                EmailSender,
                FileSaver,
                EmailsSettings,
                AzureBlobStorageSettings,
                Logger.None,
                ExecutionContext,
                UserAuthIdProvider,
                runQuartz:false);
        }

        private static void ClearDatabase(IDbConnection connection)
        {
            const string sql = "DELETE FROM app.InternalCommands " +
                               "DELETE FROM app.OutboxMessages " +
                               "DELETE FROM trees.Trees " +
                               "DELETE FROM trees.UserProfiles " +
                               "DELETE FROM trees.TreeUserProfile " +
                               "DELETE FROM trees.TreeRelations";

            connection.ExecuteScalar(sql);
        }

        public void Dispose()
        {
        }
    }
}
