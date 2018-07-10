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
        public int UserID { get; set; }
        public string Fullname { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public SelectList GetUserList()
        {
            List<AssignUserRole> userList = AssignUserRole.GetUserListFromDatabase().ToList();
            List<SelectListItem> list = userList.Select(
                x => new SelectListItem
                {
                    Text = x.Fullname,
                    Value = x.UserID.ToString()
                }).ToList();
            SelectList usr = new SelectList(list, "Value", "Text");
            return usr;
        }

        public SelectList GetRoleList()
        {
            List<AssignUserRole> roleList = AssignUserRole.GetRoleListFromDatabase().ToList();
            List<SelectListItem> list = roleList.Select(
                y => new SelectListItem
                {
                    Text = y.RoleName,
                    Value = y.RoleID.ToString()
                }).ToList();

            SelectList rle = new SelectList(list, "Value", "Text");
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
    }
}