namespace ClassLibrary1.Tests.Helpers
{
    public class FakeEntityDataStore : EntityDataStore<string, FakeEntity>
    {
        public FakeEntityDataStore(
               EntityDataStoreOptions entityDataStoreOptions) : base("fakes", entityDataStoreOptions)
        {
        }
    }
}
