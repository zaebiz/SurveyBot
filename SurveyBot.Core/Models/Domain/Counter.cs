using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SurveyBot.Core.Models.Domain
{
    public class Counter
    {
        [BsonId]
        ObjectId InternalId { get; set; } = ObjectId.Empty;

        public string Name { get; set; }
        public int Value { get; set; }
    }
}
