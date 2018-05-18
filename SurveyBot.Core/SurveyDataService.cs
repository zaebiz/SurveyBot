using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using SurveyBot.Core.Models.Domain;
using MongoDB.Driver;


namespace SurveyBot.Core
{
    public class SurveyDataService
    {
        private readonly SurveyContext _ctx;

        public SurveyDataService(SurveyContext ctx)
        {
            _ctx = ctx;
        }

        public async Task UpdateSurvey(Survey survey)
        {
            //Expression<Func<Survey, ObjectId>> ex = x => x.InternalId;
            //var filter = Builders<Survey>.Filter.Eq(x => x.InternalId, survey.InternalId);
            //var filter2 = Builders<Survey>.Filter.Eq(ex, survey.InternalId);
            //var update = Builders<Survey>.Update.Set(x => x.UpdateDate, DateTime.Now);


            await _ctx.Surveys.UpdateOneAsync(
                Builders<Survey>.Filter.Eq(x => x.InternalId, survey.InternalId),
                Builders<Survey>.Update.Set(x => x.UpdateDate, DateTime.Now),
                new UpdateOptions()
                {
                    IsUpsert = true,
                }
            );
        }
    }
}
