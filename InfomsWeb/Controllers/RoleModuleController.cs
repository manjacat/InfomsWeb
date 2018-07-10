using InfomsWeb.Models;
using InfomsWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.Controllers
{
    public class RoleModuleController : Controller
    {
        // GET: RoleModule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            int idNo = Convert.ToInt32(id);
            RoleModule rm = RoleModule.GetRoleModule(idNo);
            return View(rm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string roleId, string txtCheckList = "")
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return RedirectToAction("Index");
            }
            int idNo = Convert.ToInt32(roleId);

            try
            {
                List<string> moduleIdList = txtCheckList.Split(',').ToList();
                List<int> moduleIdListNo = moduleIdList.
                    Select(x => Convert.ToInt32(x))
                    .Distinct()
                    .ToList();
                int retVal = RoleModule.UpdateRoleModule(moduleIdListNo, idNo);
                ViewBag.Message = "Module updated successfully.";
            }
            catch
            {
                ViewBag.Message = "Update failed. Please try again later.";
            }
            RoleModule rm = RoleModule.GetRoleModule(idNo);
            return View(rm);
        }
    }
}