using InfomsWeb.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    public class RoleRPS
    {
        public RoleRPS()
        {
            Modules = ModuleTree.BuildTree();
        }

        public int ID { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }
        [Display(Name = "Default Role?")]
        public bool IsDefault { get; set; }
        public ModuleTree Modules { get; set; }

        public static List<RoleRPS> GetRoles()
        {
            RoleDataContext db = new RoleDataContext();
            List<RoleRPS> roles = db.GetRoles();
            return roles;
        }

        public static RoleRPS GetRole(int id)
        {
            RoleDataContext db = new RoleDataContext();
            RoleRPS role = db.GetRole(id);
            return role;
        }

        public int Create()
        {
            RoleDataContext db = new RoleDataContext();
            return db.CreateRole(this);
        }

        public int Update()
        {
            RoleDataContext db = new RoleDataContext();
            return db.UpdateRole(this);
        }

    }
}