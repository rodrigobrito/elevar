using System;
using Elevar.Domain.Specs;
using Elevar.Domain.Specs.BooleanOperators;
using Elevar.Tests.Domain.Specs.Utils;
using Xunit;

namespace Elevar.Tests.Domain.Specs.Operators
{
    public class OrSpecificationTests
    { 
        [Fact]
        public void Constructor_should_throw_exception_when_argument_is_null()
        {
            var spec = new AdHocSpecification<string>(x => x.Length == 1);

            Assert.Throws<ArgumentNullException>(() => new OrSpecification<string>(spec, null));
            Assert.Throws<ArgumentNullException>(() => new OrSpecification<string>(null, spec));
        }

        [Fact]
        public void Or_should_work()
        {
            var startWithE = new AdHocSpecification<string>(n => n.StartsWith("E"));
            var endsWithN = new AdHocSpecification<string>(n => n.EndsWith("n"));

            var result = new SampleRepository()
                .Find(new OrSpecification<string>(startWithE, endsWithN));

            Assert.Contains("Elton", result);
            Assert.Contains("Alison", result);
            Assert.DoesNotContain("Rodrigo", result);          
        }

        [Fact]
        public void Equals_returns_true_when_both_sides_are_equals()
        {
            var s1 = new AdHocSpecification<string>(x => x.Length > 1);
            var s2 = new AdHocSpecification<string>(x => x.Length > 2);
            var spec = s1 | s2;

            Assert.IsType<OrSpecification<string>>(spec);
            Assert.True(spec.Equals(spec));
            Assert.True(spec.Equals(s1 | s2));
            Assert.True(spec.Equals(s1 || s2)); // | or || both operators behave as ||
        }

        [Fact]
        public void Equals_returns_false_when_both_sides_are_not_equals()
        {
            var s1 = new AdHocSpecification<string>(x => x.Length > 1);
            var s2 = new AdHocSpecification<string>(x => x.Length > 2);
            var s3 = new AdHocSpecification<string>(x => x.Length > 3);
            var spec = s1 | s2;

            Assert.IsType<OrSpecification<string>>(spec);
            Assert.False(spec.Equals(null));
            Assert.False(spec.Equals(10));
            Assert.False(spec.Equals(s1));
            Assert.False(spec.Equals(s2));
            Assert.False(spec.Equals(s2 | s1)); // OrElse is not commutable
            Assert.False(spec.Equals(s1 | s3));
            Assert.False(spec.Equals(s3 | s2));
        }

        [Fact]
        public void GetHashCode_retuns_same_value_for_equal_specifications()
        {
            var s1 = new AdHocSpecification<string>(x => x.Length > 1);
            var s2 = new AdHocSpecification<string>(x => x.Length > 2);
            var s3 = new AdHocSpecification<string>(x => x.Length > 3);
            var spec1 = s1 | s2 | s3;
            var spec2 = s1 | s2 | s3;

            Assert.IsType<OrSpecification<string>>(spec1);
            Assert.IsType<OrSpecification<string>>(spec2);
            Assert.Equal(spec1.GetHashCode(), spec2.GetHashCode());
        }       
    }
}
