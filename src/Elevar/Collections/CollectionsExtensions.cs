using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevar.Collections
{
    public static class CollectionsExtensions
    {
        public static IEnumerable<T> OrderBySequence<T, TId>(this IEnumerable<T> source, IEnumerable<TId> order, Func<T, TId> idSelector)
        {
            var enumerable = source as T[] ?? source.ToArray();

            var lookup = enumerable.ToLookup(idSelector, t => t);
            var result = order.SelectMany(id => lookup[id]).ToList();

            foreach (var item in enumerable)
            {
                if (!result.Contains(item))
                    result.Add(item);
            }
            return result;
        }
    }
}
