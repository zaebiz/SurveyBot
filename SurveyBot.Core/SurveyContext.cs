using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
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
            var client = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress(settings.Value.Server, settings.Value.Port),
                Credential = MongoCredential.CreateCredential(settings.Value.Database, settings.Value.UserName, settings.Value.Password),
                ClusterConfigurator = builder =>
                {
                    builder.Subscribe(new SingleEventSubscriber<CommandStartedEvent>(TraceRequest));
                    builder.Subscribe(new SingleEventSubscriber<CommandSucceededEvent>(TraceResponse));
                }
            });

            _mongo = client.GetDatabase(settings.Value.Database);

            if (_mongo == null)
            {
                throw new ArgumentNullException("MongoDB instance is inaccessible");
            }
        }

        public SurveyContext(string connectionString, string database)
        {
            var client = new MongoClient(connectionString);
            _mongo = client.GetDatabase(database);

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

        public IMongoCollection<Counter> Counters
            => _mongo.GetCollection<Counter>("Counters");

        void TraceRequest(CommandStartedEvent command)
        {
            if (new List<string> { "isMaster", "buildInfo", "saslStart", "saslContinue", "getLastError" }.Contains(command.CommandName)) return;

            Debug.WriteLine("**********************************************");
            Debug.WriteLine($"{command.CommandName}");
            Debug.WriteLine($"{command.Command.ToJson()}");
        }

        void TraceResponse(CommandSucceededEvent command)
        {
            if (new List<string> { "isMaster", "buildInfo", "saslStart", "saslContinue", "getLastError" }.Contains(command.CommandName)) return;

            Debug.WriteLine("**********************************************");
            Debug.WriteLine($"{command.CommandName}");
            Debug.WriteLine($"{command.Reply.ToJson()}");
        }

    }
}
