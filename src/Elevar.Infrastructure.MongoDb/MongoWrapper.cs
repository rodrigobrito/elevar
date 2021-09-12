using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Elevar.Infrastructure.MongoDb
{
    public class MongoWrapper: IMongoWrapper
    {
        private readonly IMongoDatabase _mongoClient;

        public MongoWrapper(IMongoDatabase mongoClient)
        {
            _mongoClient = mongoClient ?? throw new ArgumentNullException(nameof(mongoClient));
            SetConventionPack();
        }

        private static void SetConventionPack()
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
            };
            ConventionRegistry.Register("camel case", pack, t => true);
        }

        public IMongoCollection<TDocument> CollectionMongo<TDocument>(string collectionName) => _mongoClient.GetCollection<TDocument>(collectionName);

        public List<TDocument> FindAll<TDocument>(string collectionName)
        {
            return Find<TDocument>(collectionName, w => true);
        }

        public List<TDocument> Find<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            return CollectionMongo<TDocument>(collectionName).Find(filter).ToListAsync().Result;
        }

        public List<TDocument> Find<TDocument>(string collectionName, FilterDefinition<TDocument> filter)
        {
            return CollectionMongo<TDocument>(collectionName).Find(filter).ToListAsync().Result;
        }

        public async Task<ReplaceOneResult> SaveAsync<TDocument>(string collectionName, TDocument document, Expression<Func<TDocument, bool>> filter)
        {
            return await CollectionMongo<TDocument>(collectionName).ReplaceOneAsync(filter, document, new ReplaceOptions { IsUpsert = true });
        }


        public TDocument FindOne<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            return CollectionMongo<TDocument>(collectionName).Find(filter).FirstOrDefaultAsync().Result;
        }

        public async Task<TDocument> FindOneAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            return await CollectionMongo<TDocument>(collectionName).Find(filter).FirstOrDefaultAsync();
        }

        public TDocument FindOne<TDocument>(string collectionName, FilterDefinition<TDocument> filter)
        {
            return CollectionMongo<TDocument>(collectionName).Find(filter).FirstOrDefaultAsync().Result;
        }

        public async Task<IEnumerable<BulkWriteResult<TDocument>>> BulkWriteAsync<TDocument>(string collectionName, IEnumerable<TDocument> documents, Func<TDocument, Expression<Func<TDocument, bool>>> filter, int chunksize)
        {
            var chunkedList = Chunk(documents, chunksize);
            var tasks = chunkedList.Select(c => BulkWriteAsync(collectionName, c, filter));
            return await Task.WhenAll(tasks);
        }

        public async Task<BulkWriteResult<TDocument>> BulkWriteAsync<TDocument>(string collectionName, IEnumerable<TDocument> documents, Func<TDocument, Expression<Func<TDocument, bool>>> filter)
        {
            var collection = CollectionMongo<TDocument>(collectionName);
            var options = new BulkWriteOptions { IsOrdered = false };
            IEnumerable<WriteModel<TDocument>> request;
            if (filter != null)
            {
                request = documents.Select(f => new ReplaceOneModel<TDocument>(filter(f), f) { IsUpsert = true });
            }
            else
            {
                request = documents.Select(f => new InsertOneModel<TDocument>(f));
            }
            return await collection.BulkWriteAsync(request, options);
        }

        private static IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        public async Task<List<TDocument>> FindAllAsync<TDocument>(string collectionName)
        {
            var listAllCursor = await CollectionMongo<TDocument>(collectionName).FindAsync(w => true);
            return await listAllCursor.ToListAsync();
        }

        public async Task<List<TDocument>> FindAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            var listCursor = await CollectionMongo<TDocument>(collectionName).FindAsync<TDocument>(filter);
            return await listCursor.ToListAsync();
        }

        public async Task<List<TDocument>> FindAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter)
        {
            var listCursor = await CollectionMongo<TDocument>(collectionName).FindAsync<TDocument>(filter);
            return await listCursor.ToListAsync();
        }

        public async Task UpdateManyAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update)
        {
            await CollectionMongo<TDocument>(collectionName).UpdateManyAsync(filter, update);
        }

        public async Task<TDocument> FindOneAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter)
        {
            return await CollectionMongo<TDocument>(collectionName).Find(filter).FirstOrDefaultAsync();
        }

        public IFindFluent<TDocument, TDocument> FindFluent<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            return CollectionMongo<TDocument>(collectionName).Find(filter);
        }

        public async Task<TDocument> FindOneAndUpdateAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument> options = null)
        {
            return await CollectionMongo<TDocument>(collectionName).FindOneAndUpdateAsync(filter, update, options);
        }

        public async Task<DeleteResult> DeleteOneAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter)
        {
            return await CollectionMongo<TDocument>(collectionName).DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> DeleteOneAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            return await CollectionMongo<TDocument>(collectionName).DeleteOneAsync(filter);
        }        

        public async Task CreateIndexIfNotExistsAsync<TDocument>(string collectionName, Expression<Func<TDocument, object>> field, string indexName, bool? unique = null)
        {
            await CreateIndexIfNotExistsAsync(collectionName, field, indexName, null, unique);
        }

        public async Task CreateIndexIfNotExistsAsync<TDocument>(string collectionName, string indexName, int? ttl, bool? unique = null, params Expression<Func<TDocument, object>>[] fields)
        {
            var collection = CollectionMongo<TDocument>(collectionName);
            if (collection != null)
            {
                if (!await IndexExistsAsync(collection, indexName))
                {
                    var indexKeys = new IndexKeysDefinition<TDocument>[fields.Length];

                    for (int i = 0; i < fields.Length; i++)
                    {
                        indexKeys[i] = Builders<TDocument>.IndexKeys.Ascending(fields[i]);
                    }

                    CreateIndexOptions indexOptions;

                    if (ttl.HasValue)
                        indexOptions = new CreateIndexOptions { Name = indexName, ExpireAfter = TimeSpan.FromDays(ttl.Value), Unique = unique };
                    else
                        indexOptions = new CreateIndexOptions { Name = indexName, Unique = unique };

                    var indexDefinition = Builders<TDocument>.IndexKeys.Combine(indexKeys);
                    var indexModel = new CreateIndexModel<TDocument>(indexDefinition, indexOptions);
                    await collection.Indexes.CreateOneAsync(indexModel);
                }
            }
        }

        public async Task CreateIndexIfNotExistsAsync<TDocument>(string collectionName, Expression<Func<TDocument, object>> field, string indexName, int? ttl, bool? unique = null)
        {
            var collection = CollectionMongo<TDocument>(collectionName);
            if (collection != null)
            {
                if (!await IndexExistsAsync(collection, indexName))
                {
                    CreateIndexOptions indexOptions;

                    if (ttl.HasValue)
                        indexOptions = new CreateIndexOptions { Name = indexName, ExpireAfter = TimeSpan.FromDays(ttl.Value), Unique = unique };
                    else
                        indexOptions = new CreateIndexOptions { Name = indexName, Unique = unique };

                    var indexModel = new CreateIndexModel<TDocument>(Builders<TDocument>.IndexKeys.Ascending(field), indexOptions);
                    await collection.Indexes.CreateOneAsync(indexModel);
                }
            }
        }

        public async Task<bool> IndexExistsAsync<T>(IMongoCollection<T> collection, params string[] indexesVerify)
        {
            using (var cursor = await collection.Indexes.ListAsync())
            {
                var indexes = await cursor.ToListAsync();
                foreach (BsonDocument index in indexes)
                {
                    var currentIndex = index.GetValue("key", null).ToBsonDocument();
                    if (currentIndex == null) continue;
                    var result = indexesVerify.Any(checkIndex => (currentIndex.GetValue(checkIndex, 0).ToBoolean()));
                    if (result) return true;
                }
            }
            return false;
        }

        public async Task<bool> CollectionExistsAsync(string collectionName)
        {
            var collectionList = await _mongoClient.ListCollectionsAsync();
            var collectionExists = false;

            if (collectionList != null)
            {
                await collectionList.ForEachAsync(document =>
                {
                    if (document["name"].AsString.Equals(collectionName))
                        collectionExists = true;
                });
            }
            return collectionExists;
        }

        public async Task<IMongoCollection<TDocument>> CreateCollectionIfNotExistsAsync<TDocument>(string collectionName)
        {
            return await CreateCollectionIfNotExistsAsync<TDocument>(collectionName, false, null, null);
        }

        public async Task<IMongoCollection<TDocument>> CreateCollectionIfNotExistsAsync<TDocument>(string collectionName, bool capped, long? maxSize, long? maxDocs)
        {
            var collectionExists = await CollectionExistsAsync(collectionName);
            if (collectionExists) return CollectionMongo<TDocument>(collectionName);

            if (capped && maxSize > 0)
            {
                var options = new CreateCollectionOptions
                {
                    Capped = capped,
                    MaxSize = maxSize
                };

                if (maxDocs > 0)
                    options.MaxDocuments = maxDocs;

                await _mongoClient.CreateCollectionAsync(collectionName, options);
            }
            else
            {
                await _mongoClient.CreateCollectionAsync(collectionName);
            }
            return CollectionMongo<TDocument>(collectionName);
        }

        public List<TDocument> FindPaging<TDocument>(string collectionName, FilterDefinition<TDocument> filter, int pageSize = 100, int currentPage = 1)
        {
            return CollectionMongo<TDocument>(collectionName).Find(filter).Skip((currentPage - 1) * pageSize).Limit(pageSize).ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<List<TDocument>> FindPagingAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter, int pageSize = 100, int currentPage = 1)
        {
            return await CollectionMongo<TDocument>(collectionName).Find(filter).Skip((currentPage - 1) * pageSize).Limit(pageSize).ToListAsync();
        }
    }
}