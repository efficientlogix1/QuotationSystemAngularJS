using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL;
namespace Triotech.Controllers
{
    [Authorize]
    public class ActivityLogController : Controller
    {
        // GET: ActivityLog
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Fetch()
        {
            CallBackData callBackData = (new ActivityLogLogic()).Fetch(Request.FetchPaging());

            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
    }
}