using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SurveyBot.Core.Models.Domain
{
    /// <summary>
    /// шаблон вопроса
    /// </summary>
    public class SurveyQuestion
    {
        public int Id { get; set; }
        public int QuestionType { get; set; }
        public string Text { get; set; }
        public int Index { get; set; }
    }
}