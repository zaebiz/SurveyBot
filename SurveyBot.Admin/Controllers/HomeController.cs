using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyBot.Core;
using SurveyBot.Core.Models.Domain;

namespace SurveyBot.Admin.Controllers
{    
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class SurveyController : Controller
    {
        private readonly ISurveyDataService _surveySvc;

        public SurveyController(ISurveyDataService surveySvc)
        {
            _surveySvc = surveySvc;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurvey([FromBody]Survey survey)
        {
            survey.CreateDate = DateTime.Now;
            survey = await _surveySvc.CreateSurvey(survey);
            return Json(await _surveySvc.GetSurvey(survey.InternalId.ToString()));
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
