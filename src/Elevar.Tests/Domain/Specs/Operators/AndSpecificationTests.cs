using System;
using System.Linq;
using Elevar.Domain.Specs;
using Xunit;
using Elevar.Domain.Specs.BooleanOperators;
using Elevar.Tests.Domain.Specs.Utils;

namespace Elevar.Tests.Domain.Specs.Operators
{
    public class AndSpecificationTests
    {  
        [Fact]
        public void Constructor_should_throw_exception_when_argument_is_null()
        {
            var spec = new AdHocSpecification<string>(x => x.Length == 1);

            Assert.Throws<ArgumentNullException>(() => new AndSpecification<string>(spec, null));
            Assert.Throws<ArgumentNullException>(() => new AndSpecification<string>(null, spec));
        }

        [Fact]
        public void And_should_work()
        {
            var startWithR = new AdHocSpecification<string>(n => n.StartsWith("R"));
            var endsWithO = new AdHocSpecification<string>(n => n.EndsWith("o"));
            var specfication = new AndSpecification<string>(startWithR, endsWithO);

            var result = new SampleRepository().Find(specfication).ToArray();

            Assert.All(result, item => Assert.Contains("Rodrigo", item));
            Assert.All(result, item => Assert.DoesNotContain("Elton", item));
            Assert.All(result, item => Assert.DoesNotContain("Alison", item));                       
        }

        [Fact]
        public void Equals_returns_true_when_both_sides_are_equals()
        {
            var s1 = new AdHocSpecification<string>(x => x.Length > 1);
            var s2 = new AdHocSpecification<string>(x => x.Length > 2);
            var spec = s1 & s2;

            Assert.IsType<AndSpecification<string>>(spec);            
            Assert.True(spec.Equals(s1 & s2));
            Assert.True(spec.Equals(s1 && s2)); // & or && both operators behave as &&
        }

        [Fact]
        public void Equals_returns_false_when_both_sides_are_not_equals()
        {
            var s1 = new AdHocSpecification<string>(x => x.Length > 1);
            var s2 = new AdHocSpecification<string>(x => x.Length > 2);
            var s3 = new AdHocSpecification<string>(x => x.Length > 3);
            var spec = s1 & s2;

            Assert.IsType<AndSpecification<string>>(spec);
            Assert.False(spec.Equals(null));            
            Assert.False(spec.Equals(s1));
            Assert.False(spec.Equals(s2));
            Assert.False(spec.Equals(s2 & s1)); // AndAlso is not commutable
            Assert.False(spec.Equals(s1 & s3));
            Assert.False(spec.Equals(s3 & s2));
        }

        [Fact]
        public void GetHashCode_retuns_same_value_for_equal_specifications()
        {
            var s1 = new AdHocSpecification<string>(x => x.Length > 1);
            var s2 = new AdHocSpecification<string>(x => x.Length > 2);
            var s3 = new AdHocSpecification<string>(x => x.Length > 3);
            var spec1 = s1 & s2 & s3;
            var spec2 = s1 & s2 & s3;

            Assert.IsType<AndSpecification<string>>(spec1);
            Assert.IsType<AndSpecification<string>>(spec2);
            Assert.Equal(spec1.GetHashCode(), spec2.GetHashCode());
        }
    }
}
