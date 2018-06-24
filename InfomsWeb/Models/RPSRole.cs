using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    public class RPSRole
    {
        public RPSRole()
        {            
            Modules = new List<RPSModule>();
            Modules.Add(RPSModule.GetModule());
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public List<RPSModule> Modules { get; set; }

        public static List<RPSRole> GetRoles()
        {
            List<RPSRole> roles = new List<RPSRole>();
            RPSRole r1 = new RPSRole
            {
                ID = 1,
                Name = "admin"
            };
            RPSRole r2 = new RPSRole
            {
                ID = 2,
                Name = "manager"
            };
            RPSRole r3 = new RPSRole
            {
                ID = 3,
                Name = "database_admin"
            };
            RPSRole r4 = new RPSRole
            {
                ID = 4,
                Name = "helpdesk"
            };

            roles.Add(r1);
            roles.Add(r2);
            roles.Add(r3);
            roles.Add(r4);
            return roles;
        }
    }
}