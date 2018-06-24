using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    public class RPSModule
    {
        public RPSModule()
        {
            SubModules = new List<RPSModule>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LinkURL { get; set; }
        public int SortId { get; set; }
        public int ParentId { get; set; }
        public List<RPSModule> SubModules { get; set; }

        public static RPSModule GetModule()
        {
            RPSModule m = new RPSModule
            {
                ID = 1,
                Name = "Settings",
                Description = "User Settings",
                LinkURL = "/Settings/Index",
                SortId = 0
            };
            return m;
        }

        public static List<RPSModule> GetListModule()
        {
            List<RPSModule> allModules = new List<RPSModule>();
            List<RPSModule> tempList = GetListFromDataTable();

            //insert list to their correct submodules
            allModules = Rearrange(tempList);
            //sort modules by sortId
            allModules.Sort((x, y) => x.SortId.CompareTo(y.SortId));

            return allModules;
        }

        private static List<RPSModule> GetListFromDataTable()
        {
            List<RPSModule> tempList = new List<RPSModule>();
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
                    RPSModule m = new RPSModule
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

        private static List<RPSModule> Rearrange(List<RPSModule> input)
        {
            List<RPSModule> temp2 = new List<RPSModule>();
            List<RPSModule> output = new List<RPSModule>();
            List<RPSModule> deleted = new List<RPSModule>();

            //get innermost submodule
            foreach (RPSModule m in input)
            {
                if(m.ParentId != 0)
                {
                    List<RPSModule> subModule = input
                        .FindAll(
                        item => item.ParentId == m.ID //get trunk
                        && !deleted.Contains(item)); //find subModules
                    subModule.Sort((x, y) => x.SortId.CompareTo(y.SortId)); //sort by sortId
                    m.SubModules.AddRange(subModule);
                    if (!deleted.Contains(m))
                    {
                        temp2.Add(m);
                    }
                    deleted.AddRange(subModule);
                }
                else
                {
                    temp2.Add(m);
                }
            }

            //get parent
            foreach(RPSModule j in temp2)
            {
                List<RPSModule> subModule = input
                        .FindAll(
                        item => item.ParentId == j.ID //get trunk
                        && !deleted.Contains(item)); //find subModules
                subModule.Sort((x, y) => x.SortId.CompareTo(y.SortId)); //sort by sortId
                j.SubModules.AddRange(subModule);
                if (!deleted.Contains(j))
                {
                    output.Add(j);
                }
                deleted.AddRange(subModule);
            }

            //output = temp2;

            return output;
        }
    }


}