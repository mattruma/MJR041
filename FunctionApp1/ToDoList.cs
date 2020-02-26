using FunctionApp1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class ToDoList
    {
        private readonly IToDoEntityDataStore _toDoEntityDataStore;

        public ToDoList(
            IToDoEntityDataStore toDoEntityDataStore)
        {
            _toDoEntityDataStore = toDoEntityDataStore;
        }

        [FunctionName(nameof(ToDoList))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoList)} function processed a request.");

            var toDoEntityList =
                await _toDoEntityDataStore.ListAsync();

            var toDoList =
                toDoEntityList.Select(
                    x => new ToDo(x));

            return new OkObjectResult(toDoList);
        }
    }
}
