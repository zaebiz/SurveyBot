using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using SurveyBot.Core.Models.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SurveyBot.Core.Models.Enum;


namespace SurveyBot.Core
{
    public class SurveyDataService : ISurveyDataService
    {
        private readonly SurveyContext _ctx;
        private readonly ISequenceDataService _sequence;
        private const string AutoincrementSequenceName = "survey";

        public SurveyDataService(SurveyContext ctx, ISequenceDataService sequence)
        {
            _ctx = ctx;
            _sequence = sequence;
        }

        public async Task<Survey> GetSurvey(string id)
        {
            //ObjectId internalId = ObjectId.Empty;
            //if (ObjectId.TryParse(id, out internalId))
            //{
            //    return await (await _ctx.Surveys
            //        .FindAsync(Builders<Survey>.Filter.Eq(x => x.InternalId, internalId)))
            //        .FirstAsync();
            //}

            //return null;

            return await (await _ctx.Surveys
                    .FindAsync(Builders<Survey>.Filter.Eq(x => x.Id, id)))
                    .FirstAsync();
        }

        public async Task<Survey> CreateSurvey(Survey survey)
        {
            await CreateIdForNewQuestions(survey);

            await _ctx.Surveys.InsertOneAsync(survey);
            return survey;
        }

        public async Task<Survey> UpdateSurvey(Survey survey)
        {
            if (await IsSurveyEditForbidden(survey.Id))
            {
                throw new Exception("Survey edit forbidden");
            }

            await CreateIdForNewQuestions(survey);

            var update = Builders<Survey>.Update
                .Set(x => x.UpdateDate, DateTime.Now)
                .Set(x => x.Name, survey.Name)
                .Set(x => x.Questions, survey.Questions);

            return await _ctx.Surveys.FindOneAndUpdateAsync(
                Builders<Survey>.Filter.Eq(x => x.Id, survey.Id),
                update,
                new FindOneAndUpdateOptions<Survey>() { IsUpsert = false, ReturnDocument = ReturnDocument.After}
            );

        }

        public async Task<Survey> CloseSurvey(string surveyId)
        {
            var update = Builders<Survey>.Update
                .Set(x => x.Status, (int) SurveyStatus.Closed);

            return await _ctx.Surveys.FindOneAndUpdateAsync(
                Builders<Survey>.Filter.Eq(x => x.Id, surveyId),
                update,
                new FindOneAndUpdateOptions<Survey>() { IsUpsert = false, ReturnDocument = ReturnDocument.After }
            );

        }

        private async Task CreateIdForNewQuestions(Survey survey)
        {
            if (survey.Questions != null && survey.Questions.Any())
            {
                foreach (var q in survey.Questions)
                    q.Id = q.Id != default(int) 
                        ? q.Id : await _sequence.GetNextValue(AutoincrementSequenceName);
            }
        }

        private async Task<bool> IsSurveyEditForbidden(string surveyId)
        {
            int[] activeStatuses = {(int) SurveyStatus.Active, (int) SurveyStatus.Finished};
            return activeStatuses.Contains((await GetSurvey(surveyId)).Status);
        }
    }

    public interface ISurveyDataService
    {
        Task<Survey> GetSurvey(string id);
        Task<Survey> CreateSurvey(Survey survey);
        Task<Survey> UpdateSurvey(Survey survey);
        Task<Survey> CloseSurvey(string surveyId);
    }
}
