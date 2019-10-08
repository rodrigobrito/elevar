using Elevar.Domain.Specs.ExpressionSerialization;

namespace Elevar.Tests.Domain.Specs.Utils
{
    public class FakeSerializerFactory : IExpressionSerializerFactory
    {
        private FakeSerializer _serializer = new FakeSerializer();

        public IExpressionSerializer CreateSerializer()
        {
            return _serializer;
        }
    }
}
