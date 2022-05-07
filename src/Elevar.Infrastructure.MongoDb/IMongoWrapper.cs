using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elevar.Infrastructure.MongoDb
{
    public interface IMongoWrapper
    {
        IMongoCollection<TDocument> CollectionMongo<TDocument>(string collectionName);
        List<TDocument> FindAll<TDocument>(string collectionName);
        Task<List<TDocument>> FindAllAsync<TDocument>(string collectionName);
        List<TDocument> Find<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter);
        List<TDocument> Find<TDocument>(string collectionName, FilterDefinition<TDocument> filter);
        Task<List<TDocument>> FindAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter);
        TDocument FindOne<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter);
        TDocument FindOne<TDocument>(string collectionName, FilterDefinition<TDocument> filter);
        Task<TDocument> FindOneAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter);
        Task<TDocument> FindOneAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter);
        (List<TDocument>, long) FindPaging<TDocument>(string collectionName, FilterDefinition<TDocument> filter, out long totalRecords, int pageSize = 100, int currentPage = 1);

        Task<(List<TDocument>, long)> FindPagingAsync<TDocument>(string collectionName,
                                                         FilterDefinition<TDocument> filter,
                                                         int pageSize = 100,
                                                         int currentPage = 1,
                                                         Expression<Func<TDocument, object>> field = null,
                                                         bool descending = false);
        IFindFluent<TDocument, TDocument> FindFluent<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter);
        Task<TDocument> FindOneAndUpdateAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument> options = null);

        Task<ReplaceOneResult> SaveAsync<TDocument>(string collectionName, TDocument document, Expression<Func<TDocument, bool>> filter);
        Task<BulkWriteResult<TDocument>> BulkWriteAsync<TDocument>(string collectionName, IEnumerable<TDocument> documents, Func<TDocument, Expression<Func<TDocument, bool>>> filter);
        Task<IEnumerable<BulkWriteResult<TDocument>>> BulkWriteAsync<TDocument>(string collectionName, IEnumerable<TDocument> documents, Func<TDocument, Expression<Func<TDocument, bool>>> filter, int chunksize);
        Task UpdateManyAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update);

        Task<bool> CollectionExistsAsync(string collectionName);
        Task<IMongoCollection<TDocument>> CreateCollectionIfNotExistsAsync<TDocument>(string collectionName, bool capped, long? maxSize, long? maxDocs);
        Task<IMongoCollection<TDocument>> CreateCollectionIfNotExistsAsync<TDocument>(string collectionName);

        Task<List<TDocument>> FindAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter);
        Task<DeleteResult> DeleteOneAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter);
        Task<DeleteResult> DeleteOneAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter);

        Task<bool> IndexExistsAsync<T>(IMongoCollection<T> collection, params string[] indexesVerify);
        Task CreateIndexIfNotExistsAsync<TDocument>(string collectionName, Expression<Func<TDocument, object>> field, string indexName, int? ttl = null, bool? unique = null, IndexType indexType = IndexType.Ascending);
        Task CreateIndexIfNotExistsAsync<TDocument>(string collectionName, string field, string indexName, int? ttl = null, bool? unique = null, IndexType indexType = IndexType.Ascending);
        Task CreateIndexIfNotExistsAsync<TDocument>(string collectionName, string indexName, int? ttl = null, bool? unique = null, params Expression<Func<TDocument, object>>[] fields);
        Task CreateIndexIfNotExistsAsync<TDocument>(string collectionName, Expression<Func<TDocument, object>> field, string indexName, bool? unique = null, IndexType indexType = IndexType.Ascending);
    }
}