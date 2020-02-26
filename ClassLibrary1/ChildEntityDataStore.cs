using ClassLibrary1.Helpers;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public abstract class ChildEntityDataStore<TParentKey, TKey, TEntity> : IChildEntityDataStore<TParentKey, TKey, TEntity> where TEntity : ChildEntity<TKey>, new()
    {
        protected CosmosClient _cosmosClient = null;

        protected Container _cosmosContainer = null;

        protected ChildEntityDataStore(
            string tableName,
            EntityDataStoreOptions entityDataStoreOptions)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (entityDataStoreOptions == null)
            {
                throw new ArgumentNullException(nameof(entityDataStoreOptions));
            }

            if (entityDataStoreOptions.CosmosClient == null)
            {
                throw new ArgumentNullException(nameof(entityDataStoreOptions.CosmosClient));
            }

            if (entityDataStoreOptions.DatabaseId == null)
            {
                throw new ArgumentNullException(nameof(entityDataStoreOptions.DatabaseId));
            }

            _cosmosClient =
                entityDataStoreOptions.CosmosClient;

            var cosmosDatabase =
                _cosmosClient.GetDatabase(
                    entityDataStoreOptions.DatabaseId);

            _cosmosContainer =
               cosmosDatabase.GetContainer(
                   tableName);
        }

        public async Task AddAsync(
            TParentKey parentId,
            TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id.ToString()))
            {
                throw new ArgumentNullException(nameof(entity.Id));
            }

            var itemResponse =
                await _cosmosContainer.CreateItemAsync(
                    entity,
                    new PartitionKey(parentId.ToString()));

            itemResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteByIdAsync(
            TParentKey parentId,
            TKey id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var itemResponse =
                await _cosmosContainer.DeleteItemAsync<TEntity>(
                    id.ToString(),
                    new PartitionKey(id.ToString()));

            itemResponse.EnsureSuccessStatusCode();
        }

        public async Task<TEntity> GetByIdAsync(
            TParentKey parentId,
            TKey id)
        {
            var itemResponse =
                await _cosmosContainer.ReadItemAsync<TEntity>(
                    id.ToString(),
                    new PartitionKey(id.ToString()));

            itemResponse.EnsureSuccessStatusCode();

            return itemResponse.Resource;
        }

        public async Task<IEnumerable<TEntity>> ListAsync(
            TParentKey parentId,
            string query)
        {
            var queryDefinition =
                new QueryDefinition(
                    query);

            var feedIterator =
                _cosmosContainer.GetItemQueryIterator<TEntity>(
                    queryDefinition,
                    requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(parentId.ToString()) });

            var entityList =
                new List<TEntity>();

            while (feedIterator.HasMoreResults)
            {
                entityList.AddRange(
                    await feedIterator.ReadNextAsync());
            };

            return entityList;
        }

        public async Task UpdateAsync(
            TParentKey parentId,
            TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id.ToString()))
            {
                throw new ArgumentNullException(nameof(entity.Id));
            }

            var itemResponse =
                await _cosmosContainer.ReplaceItemAsync(
                    entity,
                    entity.Id.ToString(),
                    new PartitionKey(entity.Id.ToString()));

            itemResponse.EnsureSuccessStatusCode();
        }
    }
}
