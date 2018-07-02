using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [Display(Name = "Link Url")]
        public string LinkURL { get; set; }

        [Required]
        [Display(Name = "Order")]
        public int SortId { get; set; }

        [Required]
        [Display(Name = "Parent")]
        public int ParentId { get; set; }

        public static ModuleRPS GetModule(int id)
        {
            List<ModuleRPS> modules = GetListFromDataTable().ToList();
            ModuleRPS module = modules.Where(item => item.ID == id).FirstOrDefault();
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

        public SelectList GetParentDropdown()
        {
            List<ModuleRPS> moduleList = ModuleRPS.GetListFromDataTable().ToList();
            List<SelectListItem> list = moduleList.Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ID.ToString(),
                    Selected = x.ParentId == ParentId ? true : false
                }
            ).ToList();
            list.Insert(0, new SelectListItem { Text = "No Parent", Value = "0" });
            SelectList parentList = new SelectList(list, "Value", "Text");
            return parentList;
        }

        public SelectList GetSortDropdown()
        {
            List<ModuleRPS> moduleList =
                ModuleRPS.GetListFromDataTable()
                .Where
                (item => item.ParentId == ParentId)
                .ToList();
            List<SelectListItem> list = moduleList.Select(
                x => new SelectListItem
                {
                    Text = string.Format("{0} ({1})", x.Name, x.SortId),
                    Value = x.SortId.ToString(),
                    Selected = x.SortId == SortId ? true : false
                }
            ).ToList();
            list = list.OrderBy(x => x.Value).ToList();
            list.Insert(list.Count, new SelectListItem { Text = "Most bottom", Value = (list.Count + 1).ToString() });

            SelectList parentList = new SelectList(list, "Value", "Text");
            return parentList;
        }

        public SelectList SaveSortId(int newSortId)
        {
            //first, get all SortId list, filtered by parent
            SelectList oldSort = GetSortDropdown();
            List<SelectListItem> newList = new List<SelectListItem>();
            //then loop
            foreach(var x in oldSort)
            {
                SelectListItem li = x;
                int oldVal = Convert.ToInt32(x.Value);
                if ( oldVal >= newSortId)
                {
                    int newVal = oldVal + oldVal - newSortId;
                    li.Value = newVal.ToString();
                    newList.Add(li);
                }
            }
            SelectList newSort = new SelectList(newList, "Value", "Text");
            return newSort;
        }

    }
}