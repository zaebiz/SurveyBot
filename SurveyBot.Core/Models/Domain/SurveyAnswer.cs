using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SurveyBot.Core.Models.Domain
{
    /// <summary>
    /// ответ на вопрос опроса
    /// </summary>
    public class SurveyAnswer
    {
        public int Id { get; set; }
        public ObjectId SurveyQuestionId { get; set; }
        public string Answer { get; set; }
    }
}