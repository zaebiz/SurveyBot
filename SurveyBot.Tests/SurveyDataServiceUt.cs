using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SurveyBot.Core;
using SurveyBot.Core.Models.Domain;
using SurveyBot.Core.Models.Enum;
using Xunit;

namespace SurveyBot.Tests
{
    public class SurveyDataServiceUt
    {
        private ISurveyDataService _testObj;
        private Mock<ISequenceDataService> _sequenceSvcMock;

        private const string ConnStr = "mongodb://dev2:qwertyh!@ds037611.mlab.com:37611/rusapi";
        private const string DatabaseName = "rusapi";

        public SurveyDataServiceUt()
        {            
            var ctx = new SurveyContext(ConnStr, DatabaseName);

            _sequenceSvcMock = new Mock<ISequenceDataService>();
            _sequenceSvcMock
                .Setup(x => x.GetNextValue(It.IsAny<string>()))
                .ReturnsAsync(new Random().Next());

            _testObj = new SurveyDataService(ctx, _sequenceSvcMock.Object);    
        }

        [Fact]
        public async Task CRUD_Success()
        {
            // arrange
            var survey = new Survey
            {
                Name = "Test survey",
                Status = (int)SurveyStatusEnum.New,
                CreatorName = "test creator",
                Questions = new List<SurveyQuestion>()
                {
                    new SurveyQuestion()
                    {
                        Index = 1,
                        QuestionType = 1,
                        Text = "Q1"
                    },
                    new SurveyQuestion()
                    {
                        Index = 2,
                        QuestionType = 1,
                        Text = "Q2"
                    }
                }
            };

            // Create
            survey = await _testObj.CreateSurvey(survey);

            Assert.False(string.IsNullOrEmpty(survey.Id));
            _sequenceSvcMock.Verify(
                x => x.GetNextValue(It.IsAny<string>()),
                Times.Exactly(survey.Questions.Count())
            );

            // Update
            survey.Questions = new List<SurveyQuestion>()
            {
                new SurveyQuestion() { Index = 1, QuestionType = 1, Text = "Q3" }
            };
            survey = await _testObj.UpdateSurvey(survey);
            survey = await _testObj.GetSurvey(survey.Id);

            Assert.NotNull(survey);
            Assert.Single(survey.Questions);

            // Remove
            await _testObj.CloseSurvey(survey.Id);
            survey = await _testObj.GetSurvey(survey.Id);

            Assert.Equal((int)SurveyStatusEnum.Closed, survey.Status);
        }
    }
}
