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
    public class PhoneExtController : Controller
    {
        // GET: PhoneExt
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult Save(PhoneExt phoneExt)
        {

            return Json((new PhoneExtLogic()).Save(phoneExt, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchPhoneExtByID(int PhoneExtID)
        {
            return Json((new PhoneExtLogic()).FetchPhoneExtById(PhoneExtID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Fetch()
        {
            CallBackData callBackData = (new PhoneExtLogic()).Fetch(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }




    }
}