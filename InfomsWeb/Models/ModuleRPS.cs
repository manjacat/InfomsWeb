using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    //Convert from DB to Class
    public class ModuleRPS
    {
        public ModuleRPS()
        {

        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LinkURL { get; set; }
        public int SortId { get; set; }
        public int ParentId { get; set; }

        public static ModuleRPS GetModule(int id)
        {
            //todo
            ModuleRPS module = new ModuleRPS
            {
                ID = id,
                Name = "Settings",
                Description = "This is Gay settings",
                LinkURL = "/Settings",
                ParentId = 0,
                SortId = 1
            };
            return module;
        }

        public int Save()
        {
            //TODO
            return 0;
        }

        //used in creating Nodes/Trees
        public static IEnumerable<ModuleRPS> GetListFromDataTable()
        {
            List<ModuleRPS> tempList = new List<ModuleRPS>();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Columns.Add("LinkURL");
            dt.Columns.Add("ParentId");
            dt.Columns.Add("SortId");

            dt.Rows.Add(1, "Settings", "User Settings", "/Settings/", 0, 1);
            dt.Rows.Add(2, "Admin", "this is Admin Page", "/Admin/", 0, 2);
            dt.Rows.Add(3, "User Management", "This page is for admins to create, edit and delete users", "/Users/", 2, 1);
            dt.Rows.Add(4, "Role Management", "This page is for admins to create, edit and delete roles", "/Roles/", 2, 3);
            dt.Rows.Add(5, "Module Management", "This page is for admins to create, edit and delete modules", "/Modules/", 2, 2);
            dt.Rows.Add(6, "WebMap", "WebMap", "/WebMap/", 0, 4);
            dt.Rows.Add(7, "Dashboard", "Dashboard", "/Dashboard", 0, 3);
            dt.Rows.Add(8, "Dispatch", "Dispatch", "/Dispatch/", 7, 1);
            dt.Rows.Add(9, "Dispatch Cancel", "Dispatch", "/Dispatch/Cancel/", 8, 1);
            dt.Rows.Add(10, "Dispatch Resend", "Dispatch", "/Dispatch/Resend/", 8, 2);
            dt.Rows.Add(11, "Add New User", "User", "/Users/Add", 3, 1);
            dt.Rows.Add(12, "Dispatch Reassign", "Reassign Dispatch", "/Dispatch/Resend/Reassign", 10, 1);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ModuleRPS m = new ModuleRPS
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        LinkURL = dr["LinkURL"].ToString(),
                        ParentId = Convert.ToInt32(dr["ParentId"]),
                        SortId = Convert.ToInt32(dr["SortId"])
                    };
                    tempList.Add(m);
                }
            }
            return tempList;
        }
    }
}