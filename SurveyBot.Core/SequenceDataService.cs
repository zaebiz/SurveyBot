using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SurveyBot.Core.Models.Domain;

namespace SurveyBot.Core
{
    public class SequenceDataService : ISequenceDataService
    {
        private readonly SurveyContext _ctx;

        public SequenceDataService(SurveyContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int> GetNextValue(string counterName)
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

    public interface ISequenceDataService
    {
        /// <summary>
        ///  расчитать следующий порядковый номер для "автоинкремента"
        /// </summary>
        Task<int> GetNextValue(string counterName);
    }
}
