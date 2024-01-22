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
    [Authorize]
    public class RptController : Controller
    {
        // GET: Rpt
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult BuyerReport()
        {
            return View();
        }
        public JsonResult FetchBuyerBusiness()
        {
            int buyerID = 0;
            if (User.IsInRole("Buyer"))
            {
                AspNetUser user = new AspNetUser();
                buyerID = (new GenericModel()).FetchUserProfile().Id;
            }
            CallBackData callBackData = (new RptLogic()).FetchBuyerBusinessReport(Request.FetchPaging(), buyerID);
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PreBindBuyerReport()
        {
            return Json((new RptLogic()).PreBindBuyerReport(), JsonRequestBehavior.AllowGet);
        }
    }
}