using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevar.Collections
{
    public static class EnumerableBatchingExtensions
    {
        public static IEnumerable<IEnumerable<T>> BatchesOf<T>(this IEnumerable<T> enumerable, int size)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            var enumerator = enumerable.GetEnumerator();
            var batch = new T[size];
            var batchIndex = 0;
            var actualBatchLength = 0;

            while (enumerator.MoveNext())
            {
                batch[batchIndex++] = enumerator.Current;
                actualBatchLength++;
                if (batchIndex != size) continue;
                yield return batch;
                batch = new T[size];
                batchIndex = 0;
                actualBatchLength = 0;
            }

            var ajustedLastBatch = batch.Take(actualBatchLength).ToArray();

            if (ajustedLastBatch.Any())
                yield return ajustedLastBatch;
        }
    }
}