using FunctionApp1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class ToDoGetById
    {
        private readonly IToDoEntityDataStore _toDoEntityDataStore;

        public ToDoGetById(
            IToDoEntityDataStore toDoEntityDataStore)
        {
            _toDoEntityDataStore = toDoEntityDataStore;
        }

        [FunctionName(nameof(ToDoGetById))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoGetById)} function processed a request.");

            string id = req.Query["id"];

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }


            var toDoEntity =
                await _toDoEntityDataStore.GetByIdAsync(
                    id);

            if (toDoEntity == null) return new NotFoundResult();

            var toDo =
                new ToDo(
                    toDoEntity);

            return new OkObjectResult(toDo);
        }
    }
}
