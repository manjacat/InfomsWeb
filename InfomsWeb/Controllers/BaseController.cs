using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Labelcss = "col-form-label form-control-label col-lg-3";
            base.OnActionExecuting(filterContext);
        }
    }
}