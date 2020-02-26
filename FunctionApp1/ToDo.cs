using FunctionApp1.Data;

namespace FunctionApp1
{
    public class ToDo
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public ToDo()
        {
        }

        public ToDo(
            ToDoEntity toDoEntity)
        {
            this.Id = toDoEntity.Id;
            this.Status = toDoEntity.Status;
            this.Description = toDoEntity.Description;
        }
    }
}
