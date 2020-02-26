using ClassLibrary1.Helpers;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public abstract class EntityDataStore<TKey, TEntity> : IEntityDataStore<TKey, TEntity> where TEntity : Entity<TKey>, new()
    {
        protected CosmosClient _cosmosClient = null;

        protected Container _cosmosContainer = null;

        protected EntityDataStore(
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
            TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id.ToString()))
            {
                throw new ArgumentNullException(nameof(entity.Id));
            }

            var itemResponse =
                await _cosmosContainer.CreateItemAsync(
                    entity,
                    new PartitionKey(entity.Id.ToString()));

            itemResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteByIdAsync(
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
            TKey id)
        {
            try
            {
                var itemResponse =
                    await _cosmosContainer.ReadItemAsync<TEntity>(
                        id.ToString(),
                        new PartitionKey(id.ToString()));

                itemResponse.EnsureSuccessStatusCode();

                return itemResponse.Resource;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw ex;
            }
        }

        public async Task<IEnumerable<TEntity>> ListAsync(
            string query)
        {
            var queryDefinition =
                new QueryDefinition(
                    query);

            var feedIterator =
                _cosmosContainer.GetItemQueryIterator<TEntity>(
                    queryDefinition);

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
