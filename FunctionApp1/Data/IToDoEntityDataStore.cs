using ClassLibrary1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp1.Data
{
    public interface IToDoEntityDataStore : IEntityDataStore<string, ToDoEntity>
    {
        Task<IEnumerable<ToDoEntity>> ListAsync();
    }
}
