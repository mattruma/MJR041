using ClassLibrary1;

namespace ClassLibrary1
{
    public abstract class ChildEntity<TKey> : Entity<TKey>, IChildEntity<TKey>
    {
    }
}
