using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.Controllers
{
    public class ModulesController : Controller
    {
        // GET: Modules
        public ActionResult Index()
        {
            ModuleRPS modules = ModuleRPS.BuildTree();
            return Json(modules, JsonRequestBehavior.AllowGet);
        }
    }
}