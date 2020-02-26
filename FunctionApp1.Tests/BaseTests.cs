using Bogus;
using FunctionApp1.Tests.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace FunctionApp1.Tests
{
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger _logger = LoggerHelper.CreateLogger(LoggerTypes.List);
        protected readonly Faker _faker;
        protected CosmosClient _cosmosClient;

        protected BaseTests()
        {
            // NOTE: Make sure to set these files to copy to output directory

            _configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile("appsettings.Development.json")
                 .Build();

            _faker = new Faker();

            _cosmosClient =
                new CosmosClientBuilder(
                    _configuration["AzureCosmosOptions:ConnectionString"])
                    .WithConnectionModeDirect()
                    .Build();
        }

        public Task DisposeAsync()
        {
            _cosmosClient = null;

            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosOptions:DatabaseId"]);

            await cosmosDatabase.CreateContainerIfNotExistsAsync(
                new ContainerProperties
                {
                    Id = "todos",
                    PartitionKeyPath = "/toDoId"
                });
        }
    }
}
