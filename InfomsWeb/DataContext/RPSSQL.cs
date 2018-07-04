using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace InfomsWeb.DataContext
{
    public class RPSSQL : SQLHelper
    {
        public RPSSQL()
        {
            //TODO
            //connectionString = WebConfigurationManager.AppSettings.Get("RPSDB");
            connectionString = WebConfigurationManager.ConnectionStrings["RPSDB"].ConnectionString;
        }
    }
}