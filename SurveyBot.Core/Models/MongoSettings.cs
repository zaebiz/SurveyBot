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

        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
