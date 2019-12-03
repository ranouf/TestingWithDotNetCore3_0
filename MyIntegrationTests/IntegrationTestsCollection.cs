using Xunit;

namespace MyIntegrationTests
{
    [CollectionDefinition(Constants.TEST_COLLECTION)]
    public class IntegrationTestsCollection
    {
        public IntegrationTestsCollection()
        {

        }
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
