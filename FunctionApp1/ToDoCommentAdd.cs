using FunctionApp1.Data;
using FunctionApp1.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class ToDoCommentAdd
    {
        private readonly IToDoCommentEntityDataStore _toDoCommentEntityDataStore;

        public ToDoCommentAdd(
            IToDoCommentEntityDataStore toDoCommentEntityDataStore)
        {
            _toDoCommentEntityDataStore = toDoCommentEntityDataStore;
        }

        [FunctionName(nameof(ToDoCommentAdd))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoCommentAdd)} processed a request.");

            string toDoId = req.Query["toDoId"];

            if (string.IsNullOrWhiteSpace(toDoId))
            {
                throw new ArgumentNullException(nameof(toDoId));
            }

            var toDoCommentAddOptions =
                await req.Body.DeserializeAsync<ToDoCommentAddOptions>();

            var toDoCommentEntity =
                new ToDoCommentEntity
                {
                    Body = toDoCommentAddOptions.Body,
                    CreatedOn = DateTime.UtcNow
                };

            await _toDoCommentEntityDataStore.AddAsync(
                toDoId,
                toDoCommentEntity);

            return new CreatedResult("", new ToDoComment(toDoCommentEntity));
        }
    }
}
