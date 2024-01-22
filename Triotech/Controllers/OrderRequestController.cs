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
    public class OrderRequestController : Controller
    {
        // GET: OrderRequest
        public ActionResult RequestList()
        {
            return View();
        }
        //public ActionResult RequestSetup()
        //{
        //    return View();
        //}
        public ActionResult RequesterRequestSetup()
        {
            return View();
        }
        //public ActionResult BuyerRequestSetup()
        //{
        //    return View();
        //}
        public ActionResult VendorRequestSetup()
        {
            return View();
        }
        public ActionResult BuyerPendingRequestList()
        {
            return View();
        }
        public ActionResult BuyerPendingRequestSetup()
        {
            return View();
        }
        public ActionResult BuyerSentRequestSetup()
        {
            return View();
        }
        public ActionResult BuyerRequestList()
        {
            return View();
        }
        public ActionResult VendorRequestList()
        {
            return View();
        }
        public ActionResult RequesterRequestList()
        {
            return View();
        }
        public JsonResult SaveBuyerPendingOrder(BuyerRequest buyerRequest, int statusID)//,List<int> lstVendors,List<ProductRequest> lstProductRequests)
        {
            string url = "/OrderRequest/BuyerPendingRequestList";
            return Json((new RequestsLogic()).SaveBuyerPendingOrder(buyerRequest, url, statusID, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveRequesterOrder(OrderRequest orderRequest)
        {
            string url = "/OrderRequest/RequesterRequestList";
            AspNetUser user = new AspNetUser();
            orderRequest.RequesterID = (new GenericModel()).FetchUserProfile().Id;
            return Json((new RequestsLogic()).SaveRequesterOrder(orderRequest,url, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveBuyerPrice(BuyerRequest buyerRequest)
        {
            string url = "/OrderRequest/BuyerRequestList";
            return Json((new RequestsLogic()).SaveBuyerPrice(buyerRequest, url, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveVendorRequest(VendorRequest vendorRequest)
        {
            string url = "/OrderRequest/BuyerRequestList";
            if (User.IsInRole("Vendor"))
            {
                url = "/OrderRequest/VendorRequestList"; 
            }
            return Json((new RequestsLogic()).SaveVendorRequest(vendorRequest, url, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FetchRequestByID(int requestID)
        {

            return Json((new RequestsLogic()).FetchRequestById(requestID, User.IsInRole("Requester")), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchVendorRequestByVendorRequestId(int requestID)
        {

            return Json((new RequestsLogic()).FetchVendorRequestByVendorRequestId(requestID), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult FetchVendorRequestById(int requestID)
        {
            return Json((new RequestsLogic()).FetchVendorRequestById(requestID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchRequestVendorById(int requestID)
        {
            return Json((new RequestsLogic()).FetchRequestVendorById(requestID), JsonRequestBehavior.AllowGet);
        }        
        public JsonResult FetchAdminOrders()
        {
            CallBackData callBackData = (new RequestsLogic()).FetchAdminOrders(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchBuyerPendingOrders()
        {
            int buyerID = 0;
            AspNetUser user = new AspNetUser();
            buyerID = (new GenericModel()).FetchUserProfile().Id;
            CallBackData callBackData = (new RequestsLogic()).FetchBuyerPendingOrders(Request.FetchPaging(), buyerID);
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchBuyerSentOrders()
        {
            int buyerID = 0;
            AspNetUser user = new AspNetUser();
            buyerID = (new GenericModel()).FetchUserProfile().Id;
            CallBackData callBackData = (new RequestsLogic()).FetchBuyerSentOrders(Request.FetchPaging(), buyerID);
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchRequesterOrders()
        {
            int requesterID = 0;
            AspNetUser user = new AspNetUser();
            requesterID = (new GenericModel()).FetchUserProfile().Id;
            CallBackData callBackData = (new RequestsLogic()).FetchRequesterOrders(Request.FetchPaging(), requesterID);
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchVendorOrders()
        {
            if (User.IsInRole("Admin"))
            {
                CallBackData callBackData = (new RequestsLogic()).FetchAdminOrders(Request.FetchPaging());
                return Json(callBackData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int vendorID = 0;
                AspNetUser user = new AspNetUser();
                vendorID = (new GenericModel()).FetchUserProfile().Id;
                CallBackData callBackData = (new RequestsLogic()).FetchVendorOrders(Request.FetchPaging(), vendorID);
                return Json(callBackData, JsonRequestBehavior.AllowGet);
            }

        }

        
        public JsonResult PrebindRequesterOrderSetup()
        {
            return Json((new RequestsLogic()).PrebindRequesterOrderSetup(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult PreBindBuyerRequest()
        {
            return Json((new RequestsLogic()).PreBindBuyerRequest(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult PreBindVendorRequest()
        {
            return Json((new RequestsLogic()).PreBindVendorRequest(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult PreBindRequestList()
        {
            return Json((new RequestsLogic()).PreBindRequestList(), JsonRequestBehavior.AllowGet);
        }
        
    }
}