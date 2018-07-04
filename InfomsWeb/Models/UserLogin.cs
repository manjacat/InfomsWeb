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
        [Required(ErrorMessage = "Login Name is required.", AllowEmptyStrings = false)]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public bool TryLogin()
        {
            bool isValidUser = ValidateUser(LoginName, Password);

            return isValidUser;
        }

        public static bool TryLogin(string Username, string Password)
        {
            //var isValidUser = Membership.ValidateUser(l.Username, l.Password);
            //if (isValidUser)
            //{
            //    FormsAuthentication.SetAuthCookie(l.Username, l.RememberMe);
            //    if (Url.IsLocalUrl(returnUrl))
            //    {
            //        return Redirect(returnUrl);
            //    }
            //    else
            //    {
            //        return RedirectToAction("MyProfile", "Home");
            //    }
            //}

            return true;
        }

        public bool ValidateUser(string username, string password)
        {
            RPSSQL db = new RPSSQL();
            DataTable dt = new DataTable();

            string sqlQuery = "Select ID, LOGINNAME, PASSWORD, STAFFID, NICKNAME, EMAIL, ISDEFAULT, ISACTIVE From Users " +
                "Where LOGINNAME = '" + username + "' and PASSWORD = '" + password + "'";
            dt = db.RunQuery(sqlQuery);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}