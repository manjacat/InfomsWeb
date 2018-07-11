using InfomsWeb.DataContext;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace InfomsWeb.Models
{
    public class AssignUserRole
    {
        [Required(ErrorMessage = "Please Select a User")]
        public int UserID { get; set; }
        [Display(Name = "User's Name: ")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Please Select a Role")]
        public int RoleID { get; set; }
        [Display(Name = "Assign Role: ")]
        public string RoleName { get; set; }

        public SelectList GetUserList(int id)
        {
            List<AssignUserRole> userList = AssignUserRole.GetUserListFromDatabase().ToList();
            List<SelectListItem> list = userList.Select(
                x => new SelectListItem
                {
                    Text = x.Fullname,
                    Value = x.UserID.ToString(),
                    Selected = x.UserID == id ? true : false
                }).ToList();
            SelectList usr = new SelectList(list, "Value", "Text", id);
            return usr;
        }

        public SelectList GetRoleList(int id)
        {
            List<AssignUserRole> roleList = AssignUserRole.GetRoleListFromDatabase().ToList();
            RoleDataContext db = new RoleDataContext();
            int selectedRole = db.GetRoleByUserID(id);

            List<SelectListItem> list = roleList.Select(
                y => new SelectListItem
                {
                    Text = y.RoleName,
                    Value = y.RoleID.ToString()
                }).ToList();

            SelectList rle = new SelectList(list, "Value", "Text", selectedRole);
            return rle;
        }

        public static IEnumerable<AssignUserRole> GetUserListFromDatabase()
        {
            UserDataContext db = new UserDataContext();
            List<AssignUserRole> tempList = db.GetListActiveUser();
            return tempList;
        }

        public static IEnumerable<AssignUserRole> GetRoleListFromDatabase()
        {
            RoleDataContext db = new RoleDataContext();
            List<AssignUserRole> tempList = db.GetListActiveRoles();
            return tempList;
        }

        public int SaveUserRole()
        {
            UserDataContext db = new UserDataContext();
            return db.SaveUserRoleAssign(this);
        }
    }
}