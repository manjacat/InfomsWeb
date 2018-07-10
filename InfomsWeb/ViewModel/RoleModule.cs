using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfomsWeb.Models;
using InfomsWeb.DataContext;

namespace InfomsWeb.ViewModel
{
    public class RoleModule
    {
        public RoleModule()
        {
            Role = new RoleRPS();
            Module = new List<ModuleRPS>();
        }
        public RoleRPS Role { get; set; }
        public List<ModuleRPS> Module { get; set; }
        private List<int> SelectedModules
        {
            get
            {
                var selectedModuleId = Module.Where(x => x.IsAuthorized == true).Select(y => y.ID).ToList();
                return selectedModuleId;
            }
        }

        public string ModuleArray
        {
            get
            {
                string retString = string.Join(",", SelectedModules.ToArray());
                return retString;
            }
        }

        public ModuleTree ModuleTree
        {
            get
            {
                return ModuleTree.BuildTree(Module);
            }
        }

        public static RoleModule GetRoleModule(int roleId)
        {
            RoleModuleDataContext db = new RoleModuleDataContext();
            return db.GetRoleModule(roleId);
        }

        public static int UpdateRoleModule(List<int> intList, int roleId)
        {
            RoleModuleDataContext db = new RoleModuleDataContext();
            return db.UpdateRoleModule(intList, roleId);
        }
    }
}