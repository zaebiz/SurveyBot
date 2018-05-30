using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SurveyBot.Core.Models.Domain
{
    /// <summary>
    /// шаблон вопроса
    /// </summary>
    [DebuggerDisplay("{Text} (Type={QuestionType})")]
    public class SurveyQuestion
    {
        public int Id { get; set; }
        [BsonRequired]
        public int QuestionType { get; set; }
        [BsonRequired]
        public string Text { get; set; }
        [BsonRequired]
        public int Index { get; set; }
    }
}