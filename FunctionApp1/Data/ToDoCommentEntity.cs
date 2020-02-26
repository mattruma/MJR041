using ClassLibrary1;
using Newtonsoft.Json;
using System;

namespace FunctionApp1.Data
{
    public class ToDoCommentEntity : ChildEntity<string>
    {
        [JsonProperty("toDoId")]
        public string ToDoId { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }


        public ToDoCommentEntity() : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Object = "Comment";
        }
    }
}
