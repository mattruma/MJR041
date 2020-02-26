using Microsoft.Azure.Cosmos;

namespace ClassLibrary1
{
    public class EntityDataStoreOptions
    {
        public string DatabaseId { get; set; }
        public CosmosClient CosmosClient { get; set; }

        public EntityDataStoreOptions()
        {
        }

        public EntityDataStoreOptions(
            string databaseId,
            CosmosClient cosmosClient)
        {
            this.DatabaseId = databaseId;
            this.CosmosClient = cosmosClient;
        }
    }
}
