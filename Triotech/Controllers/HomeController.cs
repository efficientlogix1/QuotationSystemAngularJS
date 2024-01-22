using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Triotech.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private TriotechDbEntities db = new TriotechDbEntities();
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var lstusers = db.AspNetUsers.ToList();
                string admins = "";
                string buyers = "";
                string vendors = "";
                string requesters = "";
                string supervisor = "";
                string management = "";
                string finalApproval = "";
                int countAdmin = lstusers.Where(u => u.AspNetRole.Name == "Admin").Count();
                int countBuyers= lstusers.Where(u => u.AspNetRole.Name == "Buyer").Count();
                int countVendors= lstusers.Where(u => u.AspNetRole.Name == "Vendor").Count();
                int countRequesters = lstusers.Where(u => u.AspNetRole.Name == "Requester").Count();
                int countSupervisor = lstusers.Where(u => u.AspNetRole.Name == "Supervisor").Count();
                int countManagement = lstusers.Where(u => u.AspNetRole.Name == "Management").Count();
                int countFinalApproval = lstusers.Where(u => u.AspNetRole.Name == "Final Approval").Count();
                ViewBag.admins = countAdmin;
                ViewBag.buyers = countBuyers;
                ViewBag.vendors = countVendors;
                ViewBag.requesters = countRequesters;
                ViewBag.supervisor = countSupervisor;
                ViewBag.management = countManagement;
                ViewBag.finalApproval = countFinalApproval;
                admins += "<strong>" + countAdmin.ToString() + "</strong>Admin Users";
                buyers += "<strong>" + countBuyers.ToString() + "</strong>Buyer Users";
                vendors += "<strong>" + countVendors.ToString() + "</strong>Vendor Users";
                requesters += "<strong>" + countRequesters.ToString() + "</strong>Requester Users";
                supervisor += "<strong>" + countSupervisor.ToString() + "</strong>Supervisor Users";
                management += "<strong>" + countManagement.ToString() + "</strong>Management Users";
                finalApproval += "<strong>" + countFinalApproval.ToString() + "</strong>Final Approval Users";
                ViewBag.Value1 = admins;
                ViewBag.Value2 = buyers;
                ViewBag.Value3 = vendors;
                ViewBag.Value4 = requesters;
                ViewBag.Value5 = supervisor;
                ViewBag.Value6 = management;
                ViewBag.Value7 = finalApproval;
            }
            if (User.IsInRole("Requester"))
            {
                int requesterID = (new Triotech.Models.GenericModel()).FetchUserProfile().Id;
                var request = db.OrderRequests.Where(p => p.RequesterID == requesterID).ToList();
                if (request != null)
                {
                    var SendToBuyer = request.Where(r => r.ActionID == 5).ToList().Count;
                    var Incomplete = request.Where(r => r.ActionID == 4).ToList().Count;
                    var Confirmed = request.Where(r => r.ActionID == 1).ToList().Count;
                    ViewBag.Value1 = "<strong>" + SendToBuyer + "</strong>Send to buyer Requests";
                    ViewBag.Value2 = "<strong>" + Incomplete + "</strong>Incomplete Requests";
                    ViewBag.Value3 = "<strong>" + Confirmed + "</strong>Confirmed Request";
                }
                else
                {
                    ViewBag.Value1 = "<strong>0</strong>Send to buyer Requests";
                    ViewBag.Value2 = "<strong>0</strong>Incomplete Requests";
                    ViewBag.Value3 = "<strong>0</strong>Confirmed Request";
                }

            }
            if (User.IsInRole("Buyer"))
            {
                int buyerID = (new Triotech.Models.GenericModel()).FetchUserProfile().Id;
                var buyerRequests = db.BuyerRequests.Where(x => x.BuyerID == buyerID).ToList();
                if (buyerRequests != null)
                {
                    var SendToVendor = buyerRequests.Where(r => r.ActionID == 3).ToList().Count;
                    var InProgress = buyerRequests.Where(r => r.ActionID == 2).ToList().Count;
                    var Confirmed = buyerRequests.Where(r => r.ActionID == 1).ToList().Count;
                    ViewBag.Value1 = "<strong>" + SendToVendor + "</strong>Send to Vendor Requests";
                    ViewBag.Value2 = "<strong>" + InProgress + "</strong>In Progress Requests";
                    ViewBag.Value3 = "<strong>" + Confirmed + "</strong>Confirmed Request";
                }
                else
                {
                    ViewBag.Value1 = "<strong>0</strong>Send to buyer Requests";
                    ViewBag.Value2 = "<strong>0</strong>In Progress Requests";
                    ViewBag.Value3 = "<strong>0</strong>Confirmed Request";
                }

            }
            if (User.IsInRole("Vendor"))
            {
                int vendorID = (new Triotech.Models.GenericModel()).FetchUserProfile().Id;
                var lstVendorRequest = db.VendorRequests.Where(p => p.VendorID == vendorID).ToList();
                int Pending = 0;
                int NotRespondent = 0;
                int Completed = 0;
                if (lstVendorRequest != null)
                {
                    List<int> lstIds = new List<int>();
                    lstVendorRequest.ForEach(m =>
                    {
                        lstIds.Add(m.BuyerRequestID);
                    });
                    var lstBuyerRequests = db.BuyerRequests.Where(a => lstIds.Contains(a.ID)).ToList();
                    List<int> lstReqIds = new List<int>();
                    lstBuyerRequests.ForEach(m =>
                    {
                        lstReqIds.Add(m.RequestID);
                    });
                    var lstRequests = db.OrderRequests.Where(r => r.ActionID != 4).ToList();
                    foreach (var item in lstBuyerRequests)
                    {
                        var request = lstRequests.FirstOrDefault(r => r.ID == item.RequestID);
                        if (request != null)
                        {
                            if (item.StatusID == 1)
                            {
                                Pending += 1;
                            }
                            if (item.StatusID == 2)
                            {
                                Completed += 1;
                            }
                            if (item.StatusID == 3)
                            {
                                NotRespondent += 1;
                            }
                        }
                    }
                }

                if (lstVendorRequest != null)
                {
                    ViewBag.Value1 = "<strong>" + NotRespondent + "</strong>Not Respondent Requests";
                    ViewBag.Value2 = "<strong>" + Completed + "</strong>Completed Requests";
                    ViewBag.Value3 = "<strong>" + Pending + "</strong>In Progress Requests";
                }
                else
                {
                    ViewBag.Value1 = "<strong>" + NotRespondent + "</strong>Not Respondent Requests";
                    ViewBag.Value2 = "<strong>" + Completed + "</strong>Completed Requests";
                    ViewBag.Value3 = "<strong>" + Pending + "</strong>In Progress Requests";
                }

            }
            if (User.IsInRole("Supervisor"))
            {
                int supervisorID = (new Triotech.Models.GenericModel()).FetchUserProfile().Id;
                var requeslst = db.OrderManagementRequests.AsNoTracking().Where(x=>x.SupervisorID == supervisorID).ToList();
                
                int countSupervisorPending = requeslst.Where(u => u.RequestStatus == "Pending").Count();
                int countSupervisorApproved = requeslst.Where(u => u.RequestStatus == "Approved By Supervisor").Count();
                int countSupervisorRejected = requeslst.Where(u => u.RequestStatus == "Rejected By Supervisor").Count();
                
                
                    ViewBag.Value1 = "<strong>" + countSupervisorApproved.ToString() + "</strong>Approved By Supervisor";
                    ViewBag.Value2 = "<strong>" + countSupervisorRejected.ToString() + "</strong>Rejected By Supervisor";
                    ViewBag.Value3 = "<strong>" + countSupervisorPending.ToString() + "</strong>Pending";
                
            }

            if (User.IsInRole("Management"))
            {
                int ManagementID = (new Triotech.Models.GenericModel()).FetchUserProfile().Id;
                var requeslst = db.OrderManagementRequests.AsNoTracking().Where(x => x.ManagementID == ManagementID).ToList();


                int countSupervisorApproved = requeslst.Where(u => u.RequestStatus == "Approved By Supervisor").Count();
                int countSupervisorRejected = requeslst.Where(u => u.RequestStatus == "Rejected By Supervisor").Count();
                int countManagementPending = requeslst.Where(u => u.RequestStatus == "Pending").Count();

                int countTotalManagementRequestPending = countSupervisorApproved + countSupervisorRejected + countManagementPending;

                int countManagementApproved = requeslst.Where(u => u.RequestStatus == "Approved By Management").Count();
                int countManagementRejected = requeslst.Where(u => u.RequestStatus == "Rejected By Management").Count();

                
                    ViewBag.Value1 = "<strong>" + countManagementApproved.ToString() + "</strong>Approved By Management";
                    ViewBag.Value2 = "<strong>" + countManagementRejected.ToString() + "</strong>Rejected By Management";
                    ViewBag.Value3 = "<strong>" + countTotalManagementRequestPending.ToString() + "</strong>Pending";
                
            }
            if (User.IsInRole("Final Approval"))
            {
                int FinalApprovalID = (new Triotech.Models.GenericModel()).FetchUserProfile().Id;
                var requeslst = db.OrderManagementRequests.AsNoTracking().Where(x => x.FinalApprovalID == FinalApprovalID).ToList();

                int countManagementApproved = requeslst.Where(u => u.RequestStatus == "Approved By Management").Count();
                int countManagementRejected = requeslst.Where(u => u.RequestStatus == "Rejected By Management").Count();

                int countSupervisorApproved = requeslst.Where(u => u.RequestStatus == "Approved By Supervisor").Count();
                int countSupervisorRejected = requeslst.Where(u => u.RequestStatus == "Rejected By Supervisor").Count();

                int countFinalApprovedPending = requeslst.Where(u => u.RequestStatus == "Pending").Count();

                int totalRequestFinalFinalApprovel = countManagementApproved + countManagementRejected + countSupervisorApproved + countSupervisorRejected + countFinalApprovedPending;

                int countFinalApprovedApproved = requeslst.Where(u => u.RequestStatus == "Approved By Final Approval").Count();
                int countFinalApprovedRejected = requeslst.Where(u => u.RequestStatus == "Rejected By Final Approval").Count();

               
                    ViewBag.Value1 = "<strong>" + countFinalApprovedApproved.ToString() + "</strong>Approved By Final Approval";
                    ViewBag.Value2 = "<strong>" + countFinalApprovedRejected.ToString() + "</strong>Rejected By Final Approval";
                    ViewBag.Value3 = "<strong>" + totalRequestFinalFinalApprovel.ToString() + "</strong>Pending";
               
            }
            return View();
        }
        public JsonResult GetBuyerData(string strDateRange)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                List<BuyerRequest> lstBuyerRequests = new List<BuyerRequest>();
                string startDate = strDateRange.Split('-')[0];
                string endDate = strDateRange.Split('-')[1];
                DateTime date1 = DateTime.ParseExact(startDate.Trim(), "dd/MMMM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime date2 = DateTime.ParseExact(endDate.Trim(), "dd/MMMM/yyyy", System.Globalization.CultureInfo.InvariantCulture).AddHours(23).AddMinutes(59);
                if (User.IsInRole("Buyer"))
                {
                    int buyerID = (new Triotech.Models.GenericModel()).FetchUserProfile().Id;
                    lstBuyerRequests = db.BuyerRequests.AsNoTracking().Where(x => x.BuyerID == buyerID && x.BuyerPrice!=null && x.QoutationDate >= date1 && x.QoutationDate <= date2).ToList();
                    if (lstBuyerRequests.Count > 0)
                    {
                        lstBuyerRequests.ForEach(x =>
                        {
                                x.StrStartDate = x.QoutationDate.Value.ToString("dd-MMMM-yyyy");
                            
                        });
                    }


                }
                if (User.IsInRole("Admin"))
                {
                    lstBuyerRequests = db.BuyerRequests.AsNoTracking().Where(x => x.BuyerPrice != null && x.QoutationDate >= date1 && x.QoutationDate <= date2).ToList();
                    if (lstBuyerRequests.Count > 0)
                    {
                        lstBuyerRequests.ForEach(x =>
                        {
                                x.StrStartDate = x.QoutationDate.Value.ToString("dd-MMMM-yyyy");
                        
                        });
                    }
                }
                returnData.Data = lstBuyerRequests;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
            }

            return Json(returnData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}