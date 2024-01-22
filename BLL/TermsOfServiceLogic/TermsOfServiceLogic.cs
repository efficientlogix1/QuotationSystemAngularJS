using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class TermsOfServiceLogic
    {

        public TriotechDbEntities db = new TriotechDbEntities();
        public Message Save(TermsOfService termsOfService, AspNetUser userprofile)
        {
            Message msg = new Message();
            try
            {
                if (termsOfService.ID != 0)
                {
                    msg.Action = "Update";
                    TermsOfService foundtermsOfService = db.TermsOfServices.FirstOrDefault(x => x.ID == termsOfService.ID);
                    foundtermsOfService.Name = termsOfService.Name;
                    foundtermsOfService.IsActive = termsOfService.IsActive;
                    db.Entry(foundtermsOfService).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                else
                {
                    msg.Action = "Save";
                    db.TermsOfServices.Add(termsOfService);
                    db.SaveChanges();

                }
                msg.MessageDetail = "TermsOfService (" + termsOfService.Name + ") has been " + msg.Action + "d";
                userprofile.activityLog(msg.Action, "<b>TermsOfService (" + termsOfService.Name + ")</b> has been " + msg.Action + "d", termsOfService.ID, "Location");
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(msg.Action, msg.MessageDetail, termsOfService.ID, "TermsOfService");
                //userProfile.LogError(ex, "BLL/UserTypeLogic/Save");
            }

            return msg;
        }
        public CallBackData Fetch(Paging paging)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters filters = paging.SearchJson.Deserialize<CommonFilters>();
                List<FetchTermOfServices_Result> listTermsOfService = db.FetchTermOfServices(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, filters.Search, filters.Active).ToList();
                callBackData = listTermsOfService.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public ReturnData FetchTermsOfServiceById(int termsOfServiceID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                TermsOfService termsOfService = db.TermsOfServices.AsNoTracking().FirstOrDefault(u => u.ID == termsOfServiceID);
                returnData.Data = termsOfService;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
                // Static.UserProfile.LogError(ex, "BLL/CashLogic/Fetch");
            }

            return returnData;

        }

    }
}
