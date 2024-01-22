using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL;
namespace Triotech.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VisitorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Fetch()
        {
            CallBackData callBackData = (new VisitorLogic()).Fetch(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult Save(Visitor visitor)
        {
            //return Json((new VisitorLogic()).Save(visitor));
            return Json("");
        }

    }
}