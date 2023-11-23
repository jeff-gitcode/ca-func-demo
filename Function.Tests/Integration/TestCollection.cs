namespace Function.Tests.Integration
{
    [CollectionDefinition(nameof(TestCollection))]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }

    [CollectionDefinition(nameof(CreateObjectCollection))]
    public class CreateObjectCollection : ICollectionFixture<CreateObjectFixture>
    {
    }

}
