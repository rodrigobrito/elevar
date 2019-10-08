using Elevar.Domain.Specs;
using Elevar.Tests.Domain.Specs.Utils;
using Xunit;

namespace Elevar.Tests.Domain.Specs
{
    public class TrueSpecificationTests
    {
        [Fact]
        public void Should_work()
        {
            var spec = new TrueSpecification<string>();

            var result = new SampleRepository().Find(spec);

            Assert.Contains("Rodrigo", result);
            Assert.Contains("Elton", result);
            Assert.Contains("Alison", result);
        }

        [Fact]
        public void Equals_returns_true_when_both_sides_are_equals()
        {
            var spec1 = new TrueSpecification<string>();
            var spec2 = new TrueSpecification<string>();

            Assert.True(spec1.Equals(spec2));
        }

        [Fact]
        public void Equals_returns_false_when_both_sides_are_not_equals()
        {
            var spec = new TrueSpecification<string>();

            Assert.False(spec.Equals(null));
            Assert.False(spec.Equals(10));
            Assert.False(spec.Equals(new AdHocSpecification<string>(x => true)));
            Assert.False(spec.Equals(new TrueSpecification<object>()));
        }

        [Fact]
        public void GetHashCode_retuns_same_value_for_equal_specifications()
        {
            var spec1 = new TrueSpecification<string>();
            var spec2 = new TrueSpecification<string>();

            Assert.Equal(spec1.GetHashCode(), spec2.GetHashCode());
        }     
    }
}
