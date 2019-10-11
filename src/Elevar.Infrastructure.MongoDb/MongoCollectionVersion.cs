using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elevar.Infrastructure.MongoDb
{
    internal class MongoCollectionVersion
    {
        [BsonId]
        public int Number { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Released { get; set; }
        public DateTime? ReleasedOn { get; set; }
    }
}
