using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace InfomsWeb.DataContext
{
    public class RoleDataContext : RPSSQL
    {
        public List<RoleRPS> GetRoles()
        {
            DataTable dt = QueryTable("select *");
            return new List<RoleRPS>();
        }
    }
}