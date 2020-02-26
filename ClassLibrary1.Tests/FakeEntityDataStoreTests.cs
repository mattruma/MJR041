using ClassLibrary1.Tests.Helpers;
using FluentAssertions;
using System;
using Xunit;

namespace ClassLibrary1.Tests
{
    public class FakeEntityDataStoreTests : BaseTests
    {
        [Fact]
        public void When_Create_And_EntityDataStoreOptionsIsNull_Then_ThrowsArgumentNullException()
        {
            // Action

            Action action = () => new FakeEntityDataStore(
                null);

            // Assert

            action.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public void When_Create_And_EntityDataStoreOptionsCloudClientIsNull_Then_ThrowsArgumentNullException()
        {
            // Arrange

            var entityDataStoreOptions =
                new EntityDataStoreOptions();

            // Action

            Action action = () => new FakeEntityDataStore(
                entityDataStoreOptions);

            // Assert

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
