using System;

namespace ClassLibrary1.Tests.Helpers
{
    public class FakeEntity : Entity<string>
    {
        public FakeEntity() : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Object = "Fake";
        }
    }
}
