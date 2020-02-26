using FunctionApp1.Data;
using FunctionApp1.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class ToDoAdd
    {
        private readonly IToDoEntityDataStore _toDoEntityDataStore;

        public ToDoAdd(
            IToDoEntityDataStore toDoEntityDataStore)
        {
            _toDoEntityDataStore = toDoEntityDataStore;
        }

        [FunctionName(nameof(ToDoAdd))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoAdd)} processed a request.");

            var toDoAddOptions =
                await req.Body.DeserializeAsync<ToDoAddOptions>();

            var toDoEntity =
                new ToDoEntity
                {
                    Status = toDoAddOptions.Status,
                    Description = toDoAddOptions.Description
                };

            await _toDoEntityDataStore.AddAsync(
                toDoEntity);

            return new CreatedResult("", new ToDo(toDoEntity));
        }
    }
}
