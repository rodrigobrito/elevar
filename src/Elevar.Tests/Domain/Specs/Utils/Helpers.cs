#pragma warning disable SYSLIB0011

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Elevar.Tests.Domain.Specs.Utils
{    
    public class Helpers
    {
        public static T SerializeAndDeserialize<T>(T obj)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }
}
