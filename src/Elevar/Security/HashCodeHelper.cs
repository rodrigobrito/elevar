using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Elevar.Tests")]
namespace Elevar.Security
{
    public struct HashCodeHelper
    {
        private readonly int _value;

        private HashCodeHelper(int value)
        {
            _value = value;
        }

        public HashCodeHelper AndEach<T>(IEnumerable<T> items)
        {
            if (items == null)
            {
                return this;
            }

            var enumerable = items as T[] ?? items.ToArray();
            var hashCode = enumerable.Any() ? enumerable.Select(GetHashCode).Aggregate(CombineHashCodes) : 0;
            return new HashCodeHelper(CombineHashCodes(_value, hashCode));
        }

        public static implicit operator int(HashCodeHelper hashCode)
        {
            return hashCode._value;
        }

        public static HashCodeHelper Of<T>(T item)
        {
            return new HashCodeHelper(GetHashCode(item));
        }

        public HashCodeHelper And<T>(T item)
        {
            return new HashCodeHelper(CombineHashCodes(_value, GetHashCode(item)));
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            unchecked
            {
                // Code copied from System.Tuple so it must be the best way to combine hash codes or at least a good one.
                return ((h1 << 5) + h1) ^ h2;
            }
        }

        private static int GetHashCode<T>(T item)
        {
            return item == null ? 0 : item.GetHashCode();
        }
    }
}