using System;
using System.Linq;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SurveyBot.Core.Models;
using SurveyBot.Core.Models.Domain;

namespace SurveyBot.Core
{
    public class SurveyContext
    {
        private readonly IMongoDatabase _mongo;

        public SurveyContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _mongo = client.GetDatabase(settings.Value.Database);

            if (_mongo == null)
            {
                throw new ArgumentNullException("MongoDB instance is inaccessible");
            }
        }

        public IMongoCollection<Survey> Surveys 
            => _mongo.GetCollection<Survey>("Surveys");

        public IQueryable SurveysQueryable
            => _mongo.GetCollection<Survey>("Surveys").AsQueryable();

        public IMongoCollection<SurveyResult> SurveyResults
            => _mongo.GetCollection<SurveyResult>("SurveyResults");

    }
}
