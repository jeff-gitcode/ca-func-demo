using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace Function.Tests
{
    internal class FixtureFactory
    {
        private static readonly ICustomization[] Customizations = { new AutoMoqCustomization(), };

        internal static IFixture Create() => Create(Customizations);

        internal static IFixture Create(params ICustomization[] customizations) =>
            new Fixture().Customize(
                new CompositeCustomization(Customizations.Union(customizations))
            );
    }
}
