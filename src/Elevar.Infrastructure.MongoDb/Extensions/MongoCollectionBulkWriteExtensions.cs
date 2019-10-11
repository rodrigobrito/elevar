using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elevar.Collections;
using MongoDB.Driver;

namespace Elevar.Infrastructure.MongoDb.Extensions
{
    public static class MongoCollectionBulkWriteExtensions
    {
        public static void ParallelBulkWrite<TDocument>(this IMongoCollection<TDocument> @this,
                                                             IEnumerable<TDocument> docs,
                                                             int batchSize, int parallelBatches,
                                                             Func<TDocument, WriteModel<TDocument>> createWriteModel,
                                                             CancellationToken cancellToken = default(CancellationToken))
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (docs == null) throw new ArgumentNullException(nameof(docs));
            if (batchSize < 1) throw new ArgumentException("'batchSize' must be at least 1.", nameof(batchSize));
            if (parallelBatches < 1) throw new ArgumentException("'parallelBatches' must be at least 1.", nameof(parallelBatches));
            if (createWriteModel == null) throw new ArgumentNullException(nameof(createWriteModel));

            using (var cancellTokenSrc = cancellToken == default(CancellationToken)
                ? new CancellationTokenSource()
                : CancellationTokenSource.CreateLinkedTokenSource(cancellToken))
            {
                try
                {
                    var bulkWriteOpts = new BulkWriteOptions { IsOrdered = false };
                    var bulkWritesEnumr = docs
                        .Select(createWriteModel)
                        .BatchesOf(batchSize)
                        .Select(x => @this.BulkWriteAsync(x, bulkWriteOpts, cancellToken))
                        .GetEnumerator();

                    Func<int, Task<BulkWriteResult<TDocument>>[]> startBulkWrites = upToN =>
                    {
                        var started = new List<Task<BulkWriteResult<TDocument>>>();
                        while (started.Count < upToN && bulkWritesEnumr.MoveNext())
                            started.Add(bulkWritesEnumr.Current);
                        return started.ToArray();
                    };

                    var runningBulkWrites = startBulkWrites(parallelBatches);

                    while (runningBulkWrites.Length > 0)
                    {
                        var complBulkWriteInx = Task.WaitAny(runningBulkWrites, cancellToken);
                        var complBulkWrite = runningBulkWrites[complBulkWriteInx];

                        if (complBulkWrite.Status != TaskStatus.RanToCompletion) // TODO: Consider parameterized error tolerance.
                            throw new Exception("Error occurred during a bulkWrite. ", complBulkWrite.Exception);

                        runningBulkWrites = runningBulkWrites
                            .Where(x => !ReferenceEquals(x, complBulkWrite))
                            .Union(startBulkWrites(1))
                            .ToArray();
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        if (!cancellTokenSrc.IsCancellationRequested)
                            cancellTokenSrc.Cancel();
                    }
                    catch (Exception)
                    {
                    }
                    throw;
                }
            }
        }
    }
}
