using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    public class RoleRPS
    {
        public RoleRPS()
        {
            Modules = ModuleRPS.BuildTree();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public ModuleRPS Modules { get; set; }

        public static List<RoleRPS> GetRoles()
        {
            List<RoleRPS> roles = new List<RoleRPS>();
            RoleRPS r1 = new RoleRPS
            {
                ID = 1,
                Name = "admin"
            };
            RoleRPS r2 = new RoleRPS
            {
                ID = 2,
                Name = "manager"
            };
            RoleRPS r3 = new RoleRPS
            {
                ID = 3,
                Name = "database_admin"
            };
            RoleRPS r4 = new RoleRPS
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