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
    public class TermsOfServiceController : Controller
    {
        // GET: TermsOfService inde
        public ActionResult Index()
        {
            return View();
        } 


        public JsonResult Save(TermsOfService termsOfService)
        {

            return Json((new TermsOfServiceLogic()).Save(termsOfService, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchtermsOfServiceByID(int termsOfServiceID)
        {
            return Json((new TermsOfServiceLogic()).FetchTermsOfServiceById(termsOfServiceID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Fetch()
        {
            CallBackData callBackData = (new TermsOfServiceLogic()).Fetch(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
    }
}