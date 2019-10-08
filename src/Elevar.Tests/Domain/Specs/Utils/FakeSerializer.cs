using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Elevar.Domain.Specs.ExpressionSerialization;

namespace Elevar.Tests.Domain.Specs.Utils
{
    public class FakeSerializer : IExpressionSerializer
    {
        private readonly Dictionary<string, Expression> _expressions = new Dictionary<string, Expression>();

        public byte[] Serialize(Expression expression)
        {
            var text = expression.ToString();
            _expressions[text] = expression;

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(text);
                writer.Flush();
                stream.Flush();
                return stream.ToArray();
            }
        }

        public Expression Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                var text = reader.ReadString();
                return _expressions[text];
            }
        }
    }
}
