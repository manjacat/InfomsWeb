using InfomsWeb.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace InfomsWeb.Models
{
    public class UserLogin
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Login Name is required.", AllowEmptyStrings = false)]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Fullname { get; set; }

        public bool RememberMe { get; set; }

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public string UserRoles { get; set; }

        public UserLogin()
        {
        }

        public UserLogin(string u)
        {
            DataTable dt = new DataTable();
            //Select ID, LOGINNAME, PASSWORD, STAFFID, FULLNAME, ISDEFAULT, ISACTIVE
            dt = GetUserByUsername(u);
            ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
            LoginName = dt.Rows[0]["LOGINNAME"].ToString();
            Fullname = dt.Rows[0]["FULLNAME"].ToString();
            IsDefault = Convert.ToBoolean(dt.Rows[0]["ISDEFAULT"].ToString());
            IsActive = Convert.ToBoolean(dt.Rows[0]["ISACTIVE"].ToString());
            UserRoles = dt.Rows[0]["USERROLE"].ToString();
        }

        public bool TryLogin()
        {
            bool isValidUser = ValidateUser(LoginName, Password);

            return isValidUser;
        }

        private bool ValidateUser(string username, string password)
        {
            RPSSQL db = new RPSSQL();
            DataTable dt = new DataTable();

            string sqlQuery = "Select ID, LOGINNAME, PASSWORD, STAFFID, EMAIL, ISDEFAULT, ISACTIVE From Users " +
                "Where LOGINNAME = '" + username + "' and PASSWORD = '" + password + "'";
            dt = db.RunQuery(sqlQuery);

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["ISACTIVE"].ToString()) != false)
                {
                    return true;
                }
            }
            return false;
        }

        private DataTable GetUserByUsername(string username)
        {
            RPSSQL db = new RPSSQL();
            DataTable dt = new DataTable();

            //string sqlQuery = "Select ID, LOGINNAME, PASSWORD, STAFFID, FULLNAME, ISDEFAULT, ISACTIVE From Users " +
            //    "Where LOGINNAME = '" + username + "'";
            string sqlQuery = "Select u.ID, u.LOGINNAME, u.PASSWORD, u.STAFFID, u.FULLNAME, u.EMAIL, u.ISDEFAULT, u.ISACTIVE , r.[NAME] as USERROLE " +
                "From ROLES r, USERROLES ur, USERS u Where u.LOGINNAME = '" + username + "' and u.ID = ur.[USER_ID] " +
                "and ur.[ROLE_ID] = r.[ID]";
            dt = db.RunQuery(sqlQuery);

            return dt;
        }

        public string GetUserFullname()
        {
            return "Nama Penuh";
        }
    }
}