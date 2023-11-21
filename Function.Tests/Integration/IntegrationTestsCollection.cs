using Function.Tests.Integration.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Function.Tests.Integration
{
    [CollectionDefinition(Name)]
    public class IntegrationTestsCollection : ICollectionFixture<TestProgram>
    {
        public const string Name = nameof(IntegrationTestsCollection);
    }
}
