using AutoFixture.Xunit2;

namespace Function.Tests
{
    public sealed class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute()
            : base(FixtureFactory.Create) { }
    }
}
