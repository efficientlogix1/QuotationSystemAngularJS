using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Triotech.Models;

namespace Triotech.Controllers
{
    public class PriorityController : Controller
    {
        // GET: Priority
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Save(Priority priority)
        {

            return Json((new PriorityLogic()).Save(priority, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchPriorityById(int priorityID)
        {
            return Json((new PriorityLogic()).FetchPriorityById(priorityID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Fetch()
        {
            CallBackData callBackData = (new PriorityLogic()).Fetch(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }








    }
}