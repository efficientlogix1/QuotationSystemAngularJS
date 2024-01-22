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
    public class LocationController : Controller
    {
        // GET: Location
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Save(Location location)
        {

            return Json((new LocationLogic()).Save(location, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchlocationByID(int locationID)
        {
            return Json((new LocationLogic()).FetchLocationById(locationID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Fetch()
        {
            CallBackData callBackData = (new LocationLogic()).Fetch(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
    }
}