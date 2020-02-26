using ClassLibrary1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp1.Data
{
    public interface IToDoCommentEntityDataStore : IChildEntityDataStore<string, string, ToDoCommentEntity>
    {
        Task<IEnumerable<ToDoCommentEntity>> ListByToDoIdAsync(
            string toDoId);
    }
}
