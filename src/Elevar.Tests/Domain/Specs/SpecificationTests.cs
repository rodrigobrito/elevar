using System;
using System.Linq.Expressions;
using Elevar.Domain.Specs;
using Elevar.Tests.Domain.Specs.Utils;
using Xunit;

namespace Elevar.Tests.Domain.Specs
{
    public class SpecificationTests
    {
        [Fact]
        public void Can_implicitly_convert_specification_to_expression()
        {
            Specification<string> spec = new AdHocSpecification<string>(s => s.Length == 2);
            Expression<Func<string, bool>> expr = spec;

            Assert.True(expr.Compile().Invoke("ab"));
            Assert.False(expr.Compile().Invoke("abcd"));
        }

        [Fact]
        public void And_operator_should_work()
        {
            var startWithR = new AdHocSpecification<string>(n => n.StartsWith("R"));
            var endsWithO = new AdHocSpecification<string>(n => n.EndsWith("o"));

            var result = new SampleRepository().Find(startWithR & endsWithO);

            Assert.Contains("Rodrigo", result);
            Assert.DoesNotContain("Elton", result);
            Assert.DoesNotContain("Alison", result);
        }

        [Fact]
        public void Or_operator_should_work()
        {
            var startWithR = new AdHocSpecification<string>(n => n.StartsWith("R"));
            var endsWithN = new AdHocSpecification<string>(n => n.EndsWith("n"));

            var result = new SampleRepository().Find(startWithR | endsWithN);

            Assert.Contains("Rodrigo", result);
            Assert.Contains("Elton", result);
            Assert.Contains("Alison", result);
        }

        [Fact]
        public void Negate_operator_should_work()
        {
            var startWithR = new AdHocSpecification<string>(n => n.StartsWith("R"));

            var result = new SampleRepository().Find(!startWithR);

            Assert.DoesNotContain("Rodrigo", result);
            Assert.Contains("Elton", result);
            Assert.Contains("Alison", result);
        }

        [Fact]
        public void AndAlso_operator_is_equivalent_to_And_operator()
        {
            var spec1 = new AdHocSpecification<string>(n => n.Length > 2);
            var spec2 = new AdHocSpecification<string>(n => n.Length < 5);

            Assert.Equal(spec1 & spec2, spec1 && spec2);
        }

        [Fact]
        public void OrElse_operator_is_equivalent_to_Or_operator()
        {
            var spec1 = new AdHocSpecification<string>(n => n.Length < 2);
            var spec2 = new AdHocSpecification<string>(n => n.Length > 5);

            Assert.Equal(spec1 | spec2, spec1 || spec2);
        }

        [Fact]
        public void Combination_of_boolean_operators_should_work()
        {
            var startWithM = new AdHocSpecification<string>(n => n.StartsWith("A"));
            var endsWithN = new AdHocSpecification<string>(n => n.EndsWith("n"));
            var containsU = new AdHocSpecification<string>(n => n.Contains("s"));

            var result = new SampleRepository().Find(startWithM | (!endsWithN & !containsU));

            Assert.Contains("Rodrigo", result);
            Assert.DoesNotContain("Elton", result);
            Assert.Contains("Alison", result);
        }
    }
}
