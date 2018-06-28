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
            Modules = ModuleTree.BuildTree();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public ModuleTree Modules { get; set; }

        public static List<RoleRPS> GetRoles()
        {
            List<RoleRPS> roles = new List<RoleRPS>();
            for (int i = 0; i < 5; i++)
            {
                RoleRPS r1 = GetRole(i);
                roles.Add(r1);
            }

            return roles;
        }

        public static RoleRPS GetRole(int id)
        {
            RoleRPS role = new RoleRPS
            {
                ID = id,
                Name = "admin"
            };
            return role;
        }

        public int Save()
        {
            //TODO
            return 0;
        }
    }
}