using System;

namespace Elevar.Infrastructure.MongoDb.Extensions
{
    public static class MongoExtensions
    {
        public static string ToMongoDate(this DateTime date)
        {
            return $"ISODate(\"{date:yyyy-MM-dd'T'HH:mm:ss.fffZ}\")";
        }
    }
}