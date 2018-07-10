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
            UserDataContext ud = new UserDataContext();
            UserLogin l = ud.GetUserByUsername(u);

            ID = l.ID;
            LoginName = l.LoginName;
            Fullname = l.Fullname;
            IsDefault = l.IsDefault;
            IsActive = l.IsActive;
            UserRoles = l.UserRoles;
        }

        public bool TryLogin()
        {
            bool isValidUser = Membership.ValidateUser(LoginName, Password);

            return isValidUser;
        }
    }
}