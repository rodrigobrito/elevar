using System;
using Elevar.Domain.Specs;
using Elevar.Domain.Specs.BooleanOperators;
using Elevar.Domain.Specs.ExpressionSerialization;
using Elevar.Tests.Domain.Specs.Utils;
using Xunit;

namespace Elevar.Tests.Domain.Specs.Operators
{
    public class NotSpecificationTests
    {
        [Fact]
        public void Constructor_should_throw_exception_when_argument_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new NotSpecification<string>(null));
        }

        [Fact]
        public void Negate_should_work()
        {
            var startWithR = new AdHocSpecification<string>(n => n.StartsWith("R"));
            var specification = new NotSpecification<string>(startWithR);

            var result = new SampleRepository().Find(specification);

            Assert.DoesNotContain("Rodrigo", result);
            Assert.Contains("Elton", result);
            Assert.Contains("Alison", result);
        }

        [Fact]
        public void Equals_return_true_when_the_negated_spec_are_equals()
        {
            var sourceSpec = new AdHocSpecification<string>(x => x.Length > 1);
            var spec = !sourceSpec;

            Assert.IsType<NotSpecification<string>>(spec);
            Assert.True(spec.Equals(spec));
            Assert.True(spec.Equals(!sourceSpec));
        }

        [Fact]
        public void Equals_return_false_when_the_negated_spec_are_not_equals()
        {
            var sourceSpec1 = new AdHocSpecification<string>(x => x.Length > 1);
            var sourceSpec2 = new AdHocSpecification<string>(x => x.Length > 2);
            var spec = !sourceSpec1;

            Assert.IsType<NotSpecification<string>>(spec);
            Assert.False(spec.Equals(null));
            Assert.False(spec.Equals(10));
            Assert.False(spec.Equals(sourceSpec1));
            Assert.False(spec.Equals(sourceSpec2));
            Assert.False(spec.Equals(!sourceSpec2));
        }

        [Fact]
        public void GetHashCode_retuns_same_value_for_equal_specifications()
        {
            var sourceSpec = new AdHocSpecification<string>(x => x.Length > 0);
            var spec1 = !sourceSpec;
            var spec2 = !sourceSpec;

            Assert.IsType<NotSpecification<string>>(spec1);
            Assert.IsType<NotSpecification<string>>(spec2);
            Assert.Equal(spec1.GetHashCode(), spec2.GetHashCode());
        }

        [Fact]
        public void Should_be_serializable()
        {
            SerializationSettings.ExpressionSerializerFactory = new FakeSerializerFactory();

            var sourceSpec = new AdHocSpecification<string>(n => n == "it fails");
            var spec = new NotSpecification<string>(sourceSpec);

            var deserializedSpec = Helpers.SerializeAndDeserialize(spec);

            Assert.IsType<NotSpecification<string>>(deserializedSpec);
            Assert.True(deserializedSpec.ToExpression().Compile().Invoke("it works"));
            Assert.False(deserializedSpec.ToExpression().Compile().Invoke("it fails"));
        }
    }
}
