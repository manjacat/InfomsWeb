using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace InfomsWeb.Models
{
    public class UserLogin
    {
        //Select ID, LOGINNAME, PASSWORD, STAFFID, NICKNAME, EMAIL, ISDEFAULT, ISACTIVE from Users;
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
            bool isValidUser = Membership.ValidateUser(LoginName, Password);

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
    }
}