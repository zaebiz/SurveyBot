using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SurveyBot.Core.Models.Domain
{
    /// <summary>
    /// шаблон опроса
    /// </summary>
    public class Survey
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonRequired]
        public string Name { get; set; }
        public int CreatorId { get; set; } = 0;
        public string CreatorName { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [BsonRequired]
        public IEnumerable<SurveyQuestion> Questions { get; set; }
    }
}
