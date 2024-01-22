using System;
using DAL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class OrderManagementLogic
    {

        public TriotechDbEntities db = new TriotechDbEntities();
        public Message Save(OrderManagementRequest orderManagementRequest, AspNetUser userprofile)
        {
            Message msg = new Message();
            try
            {
                if (orderManagementRequest.ID != 0)
                {

                    msg.Action = "Update";
                    OrderManagementRequest foundorderManagementRequest = db.OrderManagementRequests.FirstOrDefault(x => x.ID == orderManagementRequest.ID);
                    //foundorderManagementRequest.LocationID = orderManagementRequest.LocationID;
                    //foundorderManagementRequest.PhoneExtID = orderManagementRequest.PhoneExtID;
                    //foundorderManagementRequest.DepartmenttID = orderManagementRequest.DepartmenttID;
                    //foundorderManagementRequest.PriorityID = orderManagementRequest.PriorityID;
                    //foundorderManagementRequest.RequesterID = orderManagementRequest.RequesterID;
                    //foundorderManagementRequest.SupervisorID = orderManagementRequest.SupervisorID;
                    //foundorderManagementRequest.TermOfServiceID = orderManagementRequest.TermOfServiceID;
                    //foundorderManagementRequest.RequestTittle = orderManagementRequest.RequestTittle;
                    //foundorderManagementRequest.SupervisorComment = orderManagementRequest.SupervisorComment;
                    //foundorderManagementRequest.RequesterComment = orderManagementRequest.RequesterComment;
                    //foundorderManagementRequest.ManagementComment = orderManagementRequest.ManagementComment;
                    foundorderManagementRequest.RequestStatus = orderManagementRequest.RequestStatus;
                    if (userprofile.RoleId == 5)
                    {
                        foundorderManagementRequest.SupervisorComment = orderManagementRequest.SupervisorComment;
                    }
                    if (userprofile.RoleId == 6)
                    {
                        foundorderManagementRequest.ManagementComment = orderManagementRequest.ManagementComment;
                    }
                    if (userprofile.RoleId == 7)
                    {
                        foundorderManagementRequest.CEOComment = orderManagementRequest.CEOComment;
                    }
                    db.Entry(foundorderManagementRequest).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    if (userprofile.RoleId == 5 || userprofile.RoleId == 6)
                    {
                        string authority = string.Empty;
                        if (userprofile.RoleId == 5)
                        {
                            authority = "Supervisor";
                        }
                        else
                        {
                            authority = "Management";
                        }
                        SendEmailStatusUpdate(orderManagementRequest, authority);
                    }
                }

                else
                {
                    msg.Action = "Save";
                    orderManagementRequest.RequestDate = DateTime.ParseExact(orderManagementRequest.StrRequestDate, "yyyy-MM-dd h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                    orderManagementRequest.RequesterID = userprofile.Id;
                    //orderManagementRequest.RequestStatus = "Pending";
                    db.OrderManagementRequests.Add(orderManagementRequest);
                    db.SaveChanges();
                    SendEmailToRequestAsignee(orderManagementRequest, userprofile);

                }
                msg.MessageDetail = "RequestSetup (" + orderManagementRequest.RequestTittle + ") has been " + msg.Action + "d";
                userprofile.activityLog(msg.Action, "<b>RequestSetup (" + orderManagementRequest.RequestTittle + ")</b> has been " + msg.Action + "d", orderManagementRequest.ID, "Location");
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(msg.Action, msg.MessageDetail, orderManagementRequest.ID, "OrderManagementRequest");
                //userProfile.LogError(ex, "BLL/UserTypeLogic/Save");
            }

            return msg;
        }


        public CallBackData RequesterFetch(Paging paging, int requesterID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters filters = paging.SearchJson.Deserialize<CommonFilters>();
                List<FetchRequesterOrderManagementRequests_Result> listRequesterOrderManagement = db.FetchRequesterOrderManagementRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, requesterID, filters.Search, filters.Active).ToList();
                callBackData = listRequesterOrderManagement.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }


        public CallBackData SupervisorFetch(Paging paging, int SupervisorID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters filters = paging.SearchJson.Deserialize<CommonFilters>();
                List<FetchSupervisorOrderManagementRequests_Result> listSupervisor = db.FetchSupervisorOrderManagementRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, SupervisorID, filters.Search, filters.Active).ToList();
                callBackData = listSupervisor.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }


        public CallBackData ManagementFetch(Paging paging, int ManagementID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters filters = paging.SearchJson.Deserialize<CommonFilters>();
                List<FetchManagementOrderManagementRequests_Result> listManagement = db.FetchManagementOrderManagementRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, filters.Search, filters.Active, ManagementID).ToList();
                callBackData = listManagement.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }



        public CallBackData FinalApprovalFetch(Paging paging,int FinalApprovalID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters filters = paging.SearchJson.Deserialize<CommonFilters>();
                List<FetchFinalAprovalOrderManagementRequests_Result> listManagement = db.FetchFinalAprovalOrderManagementRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, filters.Search, filters.Active,FinalApprovalID).ToList();
                callBackData = listManagement.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }


        public CallBackData RequesterRequestList(Paging paging)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters filters = paging.SearchJson.Deserialize<CommonFilters>();
                List<FetchCompleteRequesterRequestList_Result> listRequesterRequest = db.FetchCompleteRequesterRequestList(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, filters.Search, filters.Active).ToList();
                callBackData = listRequesterRequest.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }







        public ReturnData FetchOrderManagementRequestById(int OrderManagementRequestId)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                OrderManagementRequest orderManagementRequest = db.OrderManagementRequests.AsNoTracking().FirstOrDefault(u => u.ID == OrderManagementRequestId);
                orderManagementRequest.RequesterName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x=>x.Id==orderManagementRequest.RequesterID).Name;
               // orderManagementRequest.SupervisorName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == orderManagementRequest.SupervisorID).Name;
                orderManagementRequest.ManagementName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == orderManagementRequest.ManagementID).Name;
                orderManagementRequest.FinalApprovalName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == orderManagementRequest.FinalApprovalID).Name;
                orderManagementRequest.StrRequestDate = orderManagementRequest.RequestDate.ToString("yyyy-MM-dd h:mm tt");
                returnData.Data = orderManagementRequest;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
                // Static.UserProfile.LogError(ex, "BLL/CashLogic/Fetch");
            }

            return returnData;

        }

        public ReturnData PrebindOrderManagementRequestSetup(bool isSupervisor, bool isManagement,bool isCEO)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                ReturnData lstFormStatuses = new ReturnData();
                if (isSupervisor)
                {

                    lstFormStatuses = SupervisorStatuses();
                }
                else
                {
                    if (isManagement)
                    {

                        lstFormStatuses = ManagementStatuses();
                    }
                    else
                    {
                        if (isCEO)
                        {
                            lstFormStatuses = CEOStatuses();
                        }
                    }
                }
                var lstReturn = new
                {
                    lstDepartment = db.Departments.AsNoTracking().Where(x => x.IsActive == true).ToList(),
                    lstLocation = db.Locations.AsNoTracking().Where(x => x.IsActive == true).ToList(),
                    lstTermsOfService = db.TermsOfServices.AsNoTracking().Where(x => x.IsActive == true).ToList(),
                    lstPriority = db.Priorities.AsNoTracking().Where(x => x.IsActive == true).ToList(),
                    lstPhoneExt = db.PhoneExts.AsNoTracking().Where(x => x.IsActive == true).ToList(),
                    lstSupervisor = (new Generic()).FetchActiveUserByRoleName("Supervisor"),
                    lstManagement =  (new Generic()).FetchActiveUserByRoleName("Management"),
                    lstFinalApproval = (new Generic()).FetchActiveUserByRoleName("Final Approval"),
                    lstStatuses = lstFormStatuses
                };
                returnData.Data = lstReturn;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
            }

            return returnData;
        }
        public ReturnData ManagementStatuses()
        {
            ReturnData returnData = new ReturnData();
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                List<string> lstStatus = new List<string>();
                lstStatus.Add("Pending");
                lstStatus.Add("Approved By Management");
                lstStatus.Add("Rejected By Management");
                returnData.Data = lstStatus;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
            }

            return returnData;
        }
        public ReturnData SupervisorStatuses()
        {
            ReturnData returnData = new ReturnData();
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                List<string> lstStatus = new List<string>();
               // lstStatus.Add("Pending");
                lstStatus.Add("Approved By Supervisor");
                lstStatus.Add("Rejected By Supervisor");
                returnData.Data = lstStatus;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
            }

            return returnData;
        }
        public ReturnData CEOStatuses()
        {
            ReturnData returnData = new ReturnData();
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                List<string> lstStatus = new List<string>();
                lstStatus.Add("Pending");
                lstStatus.Add("Approved By Final Approval");
                lstStatus.Add("Rejected By Final Approval");
                returnData.Data = lstStatus;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
            }

            return returnData;
        }
        public ReturnData SearchStatuses()
        {
            ReturnData returnData = new ReturnData();
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                List<string> lstStatus = new List<string>();
                lstStatus.Add("Pending");
                lstStatus.Add("Approved By Management");
                lstStatus.Add("Rejected By Management");
                lstStatus.Add("Approved By Supervisor");
                lstStatus.Add("Rejected By Supervisor");
                lstStatus.Add("Approved By Final Approval");
                lstStatus.Add("Rejected By Final Approval");
                returnData.Data = lstStatus;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
            }

            return returnData;
        }
        public void SendEmailStatusUpdate(OrderManagementRequest model, string authority)
        {
            AspNetUser user = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == model.RequesterID);
            EmailTemplate emailTemplate = db.EmailTemplates.AsEnumerable().FirstOrDefault(x => x.Subject == "Request Status Update");
            emailTemplate.To = string.Join(",", emailTemplate.To, user.Email);
            emailTemplate.MessageBody = emailTemplate.MessageBody.Replace("{#!Name!#}", user.Name).Replace("{#!Authority!#}", authority).Replace("{#!Title!#}", model.RequestTittle).Replace("{#!Status!#}", model.RequestStatus);
            emailTemplate.SendEmail();
        }
        public void SendEmailToRequestAsignee(OrderManagementRequest model, AspNetUser requester)
        {
            AspNetUser supervisor = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == model.SupervisorID);
            EmailTemplate emailTemplate = db.EmailTemplates.AsEnumerable().FirstOrDefault(x => x.Subject == "New Request Alert");
            emailTemplate.To = string.Join(",", emailTemplate.To, supervisor.Email);
            emailTemplate.MessageBody = emailTemplate.MessageBody.Replace("{#!Name!#}", supervisor.Name).Replace("{#!Requester!#}", requester.Name).Replace("{#!Title!#}", model.RequestTittle).Replace("{#!Status!#}", model.RequestStatus);
            emailTemplate.SendEmail();
            AspNetUser management = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == model.ManagementID);
            emailTemplate = db.EmailTemplates.AsEnumerable().FirstOrDefault(x => x.Subject == "New Request Alert");
            emailTemplate.To = string.Join(",", emailTemplate.To, management.Email);
            emailTemplate.MessageBody = emailTemplate.MessageBody.Replace("{#!Name!#}", management.Name).Replace("{#!Requester!#}", requester.Name).Replace("{#!Title!#}", model.RequestTittle).Replace("{#!Status!#}", model.RequestStatus);
            emailTemplate.SendEmail();
            AspNetUser finalapproval = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == model.FinalApprovalID);
            emailTemplate = db.EmailTemplates.AsEnumerable().FirstOrDefault(x => x.Subject == "New Request Alert");
            emailTemplate.To = string.Join(",", emailTemplate.To, finalapproval.Email);
            emailTemplate.MessageBody = emailTemplate.MessageBody.Replace("{#!Name!#}", finalapproval.Name).Replace("{#!Requester!#}", requester.Name).Replace("{#!Title!#}", model.RequestTittle).Replace("{#!Status!#}", model.RequestStatus);
            emailTemplate.SendEmail();
        }

    }
}
