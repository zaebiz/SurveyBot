using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using SurveyBot.Core.Models.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace SurveyBot.Core
{
    public class SurveyDataService : ISurveyDataService
    {
        private readonly SurveyContext _ctx;

        public SurveyDataService(SurveyContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Survey> GetSurvey(string id)
        {
            ObjectId internalId = ObjectId.Empty;
            if (ObjectId.TryParse(id, out internalId))
            {
                return await (await _ctx.Surveys
                    .FindAsync(Builders<Survey>.Filter.Eq(x => x.InternalId, internalId)))
                    .FirstAsync();
            }

            return null;
        }

        public async Task<Survey> CreateSurvey(Survey survey)
        {
            survey.Id = await GetNextSequenceValue("survey");
            await _ctx.Surveys.InsertOneAsync(survey);
            return survey;

            //return await _ctx.Surveys.FindOneAndReplaceAsync(
            //    Builders<Survey>.Filter.Eq(x => x.InternalId, survey.InternalId),
            //    survey,
            //    new FindOneAndReplaceOptions<Survey> {IsUpsert = true, ReturnDocument = ReturnDocument.After}
            //);
        }

        public async Task<Survey> UpdateSurvey(Survey survey)
        {
            var update = Builders<Survey>.Update
                .Set(x => x.UpdateDate, DateTime.Now)
                .Set(x => x.Name, survey.Name)
                .Set(x => x.Questions, survey.Questions);

            return await _ctx.Surveys.FindOneAndUpdateAsync(
                Builders<Survey>.Filter.Eq(x => x.InternalId, survey.InternalId),
                update,
                new FindOneAndUpdateOptions<Survey>() { IsUpsert = false, ReturnDocument = ReturnDocument.After}
            );

        }

        /// <summary>
        ///  расчитать следующий порядковый номер для "автоинкремента"
        /// </summary>
        public async Task<int> GetNextSequenceValue(string counterName)
        {
            var counter = await _ctx.Counters.FindOneAndUpdateAsync(
                Builders<Counter>.Filter.Eq(x => x.Name, counterName),
                Builders<Counter>.Update.Inc(x => x.Value, 1),
                new FindOneAndUpdateOptions<Counter>()
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = true,
                }
            );

            return counter.Value;
        }
    }

    public interface ISurveyDataService
    {
        Task<Survey> GetSurvey(string id);
        Task<Survey> CreateSurvey(Survey survey);
        Task<Survey> UpdateSurvey(Survey survey);
    }
}
