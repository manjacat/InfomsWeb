using InfomsWeb.DataContext;
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

        [Required]
        [Display(Name = "Module Name")]
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

        [Display(Name = "Authorized?")]
        public bool IsAuthorized { get; set; }

        public static ModuleRPS GetModule(int id)
        {
            if (id > 0)
            {
                List<ModuleRPS> modules = GetListFromDataTable().ToList();
                ModuleRPS module = modules.Where(item => item.ID == id).FirstOrDefault();
                return module;
            }
            else
            {
                ModuleRPS module = new ModuleRPS
                {
                    ID = 0,
                    SortId = 0
                };
                return module;
            }
        }

        //used in creating ModuleTree
        public static IEnumerable<ModuleRPS> GetListFromDataTable()
        {
            RoleRPS role = RoleRPS.GetRoleByUsername(HttpContext.Current.User.Identity.Name);
            ModuleDataContext db = new ModuleDataContext();
            List<ModuleRPS> tempList = db.GetListModules(role.ID);
            return tempList;
        }

        private List<ModuleRPS> RemoveFromList(List<ModuleRPS> modules)
        {
            foreach (ModuleRPS mod in modules)
            {
                if (mod.ID == ID)
                {
                    modules.Remove(mod);
                    //exit the loop
                    return modules;
                }
            }
            return modules;
        }

        public SelectList GetParentDropdown()
        {
            List<ModuleRPS> moduleList = ModuleRPS.GetListFromDataTable().ToList();
            //remove self from list of dropdown 
            //(prevent selecting self as parent)
            moduleList = RemoveFromList(moduleList);

            List<SelectListItem> list = moduleList.Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ID.ToString(),
                    Selected = x.ParentId == ParentId ? true : false
                }
            ).ToList();
            list.Insert(0, new SelectListItem { Text = "Root", Value = "0" });
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
                    Text = Name == x.Name ? string.Format("Current ({0})", x.SortId) : string.Format("{0} ({1})", x.Name, x.SortId),
                    Value = x.SortId.ToString(),
                    Selected = x.SortId == SortId ? true : false
                }
            ).ToList();
            list = list.OrderBy(x => x.Value).ToList();
            //kalau module tu last,
            //tak payah letak bottom
            if (SortId == 0)
            {
                list.Insert(list.Count, new SelectListItem { Text = string.Format("New ({0})", list.Count + 1), Value = (list.Count + 1).ToString() });
            }

            SelectList parentList = new SelectList(list, "Value", "Text");
            return parentList;
        }

        public SelectList SaveSortId(int newSortId)
        {
            //first, get all SortId list, filtered by parent
            SelectList oldSort = GetSortDropdown();
            List<SelectListItem> newList = new List<SelectListItem>();
            //then loop
            foreach (var x in oldSort)
            {
                SelectListItem li = x;
                int oldVal = Convert.ToInt32(x.Value);
                if (oldVal >= newSortId)
                {
                    int newVal = oldVal + oldVal - newSortId;
                    li.Value = newVal.ToString();
                    newList.Add(li);
                }
            }
            SelectList newSort = new SelectList(newList, "Value", "Text");
            return newSort;
        }

        public int Update()
        {
            ModuleDataContext db = new ModuleDataContext();
            return db.UpdateModule(this);
        }

        public int Create()
        {
            ModuleDataContext db = new ModuleDataContext();
            return db.CreateModule(this);
        }
    }
}