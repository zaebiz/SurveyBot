using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SurveyBot.Core.Models
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
