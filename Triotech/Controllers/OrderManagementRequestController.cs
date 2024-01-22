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
    public class OrderManagementRequestController : Controller
    {
        // GET: OrderManagementRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RequestSetup()
        {
            return View();
        }

        public JsonResult Save(OrderManagementRequest orderManagementRequest)
        {

            return Json((new OrderManagementLogic()).Save(orderManagementRequest, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FetchorderManagementRequestByID(int orderManagementRequestId)
        {
            return Json((new OrderManagementLogic()).FetchOrderManagementRequestById(orderManagementRequestId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PrebindorderManagementRequestSetup()
        {
            bool isManagement = false;
            bool isSupervisor = false;
            bool isCEO = false;
            if (User.IsInRole("Supervisor"))
            {
                isSupervisor = true;
            }
            else  
            {
                if (User.IsInRole("Management"))
                {
                    isManagement = true;

                }
                else
                {
                    if (User.IsInRole("Final Approval"))
                    {
                        isCEO = true;

                    }
                }
            }
            return Json((new OrderManagementLogic()).PrebindOrderManagementRequestSetup(isSupervisor,isManagement,isCEO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequesterDataFetch()
        {
            return View();
        }
        public JsonResult RequesterListFetch()
        {
            int requesterID=  (new GenericModel()).FetchUserProfile().Id;
            CallBackData callBackData = (new OrderManagementLogic()).RequesterFetch(Request.FetchPaging(), requesterID);
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupervisorDataFetch()
        {
            return View();

        }
        public JsonResult SupervisorListFetch()
        {
            int supervisorID = (new GenericModel()).FetchUserProfile().Id;
            CallBackData callBackData = (new OrderManagementLogic()).SupervisorFetch(Request.FetchPaging(), supervisorID);
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ManagementDataFetch()
        {
            return View();
        }
        public JsonResult ManagementListFetch()
        {
            int ManagementID = (new GenericModel()).FetchUserProfile().Id;
            CallBackData callBackData = (new OrderManagementLogic()).ManagementFetch(Request.FetchPaging(),ManagementID);
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinalApprovalDataFetch()
        {
            return View();
        }

        public JsonResult FinalApprovalListFetch()
        {
            int FinalApprovalID = (new GenericModel()).FetchUserProfile().Id;
            CallBackData callBackData = (new OrderManagementLogic()).FinalApprovalFetch(Request.FetchPaging(),FinalApprovalID);
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequesterRequestCompleteListDataFetch()
        {
            return View();
        }


        

        public JsonResult RequesterRequestCompleteListFetch()
        {
            
            CallBackData callBackData = (new OrderManagementLogic()).RequesterRequestList(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PreBindListing()
        {
            return Json((new OrderManagementLogic()).SearchStatuses(), JsonRequestBehavior.AllowGet);
        }

    }
}