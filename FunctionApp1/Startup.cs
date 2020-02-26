using ClassLibrary1;
using FunctionApp1.Data;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]
namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(
            IFunctionsHostBuilder builder)
        {
            var services =
                builder.Services;

            var entityDataStoreOptions =
                new EntityDataStoreOptions();

            var cosmosClient =
                new CosmosClientBuilder(
                        Environment.GetEnvironmentVariable("AzureCosmosOptions:ConnectionString"))
                    .WithConnectionModeDirect()
                    .Build();

            entityDataStoreOptions.CosmosClient =
                cosmosClient;

            entityDataStoreOptions.DatabaseId =
                Environment.GetEnvironmentVariable("AzureCosmosOptions:DatabaseId");

            services.AddSingleton(entityDataStoreOptions);

            services.AddTransient<IToDoEntityDataStore, ToDoEntityDataStore>();
            services.AddTransient<IToDoCommentEntityDataStore, ToDoCommentEntityDataStore>();
        }
    }
}
