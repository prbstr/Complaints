using Complaints.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Complaints.Data.DataModels
{
    public class Authentication {
        public string Secret { get; set; }
    }

    public class ConnectionStrings
    {
        public string ComplaintsConnectionString { get; set; }
    }

    public class AppSettingsDataModel
    {
        public Authentication Authentication { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }
}
