using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace InfomsWeb.Security
{
    // kena add roleManager kat web.config baru boleh guna
    public class CustomRoleProvider : RoleProvider
    {        
        public override string[] GetRolesForUser(string username)
        {
            //TODO : edit method ini supaya boleh dapat role dari username
            string[] roles = null;
            switch (username)
            {
                case ("admin"):
                    roles = new string[] { "admin", "manager", "user" }; 
                    break;
                case ("darrel"):
                    roles = new string[] { "manager", "user" };
                    break;
                default:
                    roles = new string[] { "user" };
                    break;
            }
            return roles;
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}