using ClassLibrary1;
using FluentAssertions;
using FunctionApp1.Data;
using FunctionApp1.Tests.Helpers;
using Microsoft.Azure.Cosmos;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunctionApp1.Tests
{
    public class ToDoEntityDataStoreTests : BaseTests
    {
        [Fact]
        public async Task When_AddAsync()
        {
            // Arrange

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _configuration["AzureCosmosOptions:DatabaseId"]
                };

            var toDoEntityDataStore =
                new ToDoEntityDataStore(
                    entityDataStoreOptions);

            var toDoEntity =
                _faker.GenerateToDoEntity();

            // Action

            await toDoEntityDataStore.AddAsync(
                toDoEntity);

            // Assert

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosOptions:DatabaseId"]);

            var cosmosContainer =
                cosmosDatabase.GetContainer("todos");

            var itemResponse =
                await cosmosContainer.ReadItemAsync<ToDoEntity>(
                    toDoEntity.Id,
                    new PartitionKey(toDoEntity.Id));

            itemResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var toDoEntityFetched =
                itemResponse.Resource;

            toDoEntityFetched.Should().NotBeNull();
            toDoEntityFetched.Id.Should().Be(toDoEntity.Id);
            toDoEntityFetched.ToDoId.Should().Be(toDoEntity.ToDoId);
            toDoEntityFetched.ToDoId.Should().Be(toDoEntityFetched.Id);
            toDoEntityFetched.Status.Should().Be(toDoEntity.Status);
            toDoEntityFetched.Description.Should().Be(toDoEntity.Description);
        }

        [Fact]
        public async Task When_GetByIdAsync()
        {
            // Arrange

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosOptions:DatabaseId"]);

            var cosmosContainer =
                cosmosDatabase.GetContainer("todos");

            var toDoEntity =
                _faker.GenerateToDoEntity();

            await cosmosContainer.CreateItemAsync(
                toDoEntity,
                new PartitionKey(toDoEntity.ToDoId));

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = cosmosDatabase.Id
                };

            var toDoEntityDataStore =
                new ToDoEntityDataStore(
                    entityDataStoreOptions);

            // Action

            var toDoEntityFetched =
                await toDoEntityDataStore.GetByIdAsync(
                    toDoEntity.Id);

            // Assert

            toDoEntityFetched.Should().NotBeNull();
            toDoEntityFetched.Id.Should().Be(toDoEntity.Id);
            toDoEntityFetched.ToDoId.Should().Be(toDoEntity.ToDoId);
            toDoEntityFetched.ToDoId.Should().Be(toDoEntityFetched.Id);
            toDoEntityFetched.Status.Should().Be(toDoEntity.Status);
            toDoEntityFetched.Description.Should().Be(toDoEntity.Description);
        }

        [Fact]
        public async Task When_GetByIdAsync_And_EntityDoesNotExist_Then_ReturnNull()
        {
            // Arrange

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _configuration["AzureCosmosOptions:DatabaseId"]
                };

            var toDoEntityDataStore =
                new ToDoEntityDataStore(
                    entityDataStoreOptions);

            // Action

            var toDoEntity =
                await toDoEntityDataStore.GetByIdAsync(
                    "IMABADID");

            // Assert

            toDoEntity.Should().BeNull();
        }

        [Fact]
        public async Task When_DeleteByIdAsync()
        {
            // Arrange

            var toDoEntity =
                _faker.GenerateToDoEntity();

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosOptions:DatabaseId"]);

            var cosmosContainer =
                cosmosDatabase.GetContainer("todos");

            await cosmosContainer.CreateItemAsync(
                toDoEntity,
                new PartitionKey(toDoEntity.ToDoId));

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _configuration["AzureCosmosOptions:DatabaseId"]
                };

            var toDoEntityDataStore =
                new ToDoEntityDataStore(
                    entityDataStoreOptions);

            // Action

            await toDoEntityDataStore.DeleteByIdAsync(
                toDoEntity.Id);

            // Assert

            Func<Task> action = async () =>
                await cosmosContainer.ReadItemAsync<ToDoEntity>(
                    toDoEntity.Id,
                    new PartitionKey(toDoEntity.Id));

            action.Should().Throw<CosmosException>();
        }

        [Fact]
        public async Task When_UpdateAsync()
        {
            // Arrange

            var toDoEntity =
                _faker.GenerateToDoEntity();

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosOptions:DatabaseId"]);

            var cosmosContainer =
                cosmosDatabase.GetContainer("todos");

            await cosmosContainer.CreateItemAsync(
                toDoEntity,
                new PartitionKey(toDoEntity.ToDoId));

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _configuration["AzureCosmosOptions:DatabaseId"]
                };

            var toDoEntityDataStore =
                new ToDoEntityDataStore(
                    entityDataStoreOptions);

            toDoEntity.Description = _faker.Lorem.Paragraph(1);

            // Action

            await toDoEntityDataStore.UpdateAsync(
                toDoEntity);

            // Assert

            var itemResponse =
                await cosmosContainer.ReadItemAsync<ToDoEntity>(
                    toDoEntity.Id,
                    new PartitionKey(toDoEntity.Id));

            itemResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var toDoEntityUpdated =
                itemResponse.Resource;

            toDoEntityUpdated.Should().NotBeNull();
            toDoEntityUpdated.Description.Should().Be(toDoEntity.Description);
        }

        [Fact]
        public async Task When_ListAsync()
        {
            // Arrange

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    _configuration["AzureCosmosOptions:DatabaseId"]);

            var cosmosContainer =
                cosmosDatabase.GetContainer("todos");

            for (var i = 0; i < 3; i++)
            {
                var toDoEntity =
                    _faker.GenerateToDoEntity();

                await cosmosContainer.CreateItemAsync(
                    toDoEntity,
                    new PartitionKey(toDoEntity.ToDoId));
            }

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    CosmosClient = _cosmosClient,
                    DatabaseId = _configuration["AzureCosmosOptions:DatabaseId"]
                };

            var toDoEntityDataStore =
                new ToDoEntityDataStore(
                    entityDataStoreOptions);

            // Action

            var toDoEntityFetchedList =
                await toDoEntityDataStore.ListAsync();

            // Assert

            toDoEntityFetchedList.Should().NotBeNull();
            toDoEntityFetchedList.Count().Should().BeGreaterOrEqualTo(3);
        }
    }
}