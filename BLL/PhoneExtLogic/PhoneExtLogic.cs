using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
  public  class  PhoneExtLogic
    {

        public TriotechDbEntities db = new TriotechDbEntities();
        public Message Save(PhoneExt phoneExt, AspNetUser userprofile)
        {
            Message msg = new Message();
            try
            {
                if (phoneExt.ID != 0)
                {
                    msg.Action = "Update";
                   PhoneExt foundPhoneExt = db.PhoneExts.FirstOrDefault(x => x.ID == phoneExt.ID);
                    foundPhoneExt.Code = phoneExt.Code;
                    foundPhoneExt.IsActive = phoneExt.IsActive;
                    db.Entry(foundPhoneExt).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                else
                {
                    msg.Action = "Save";
                    db.PhoneExts.Add(phoneExt);
                    db.SaveChanges();

                }
                msg.MessageDetail = "PhoneExt (" + phoneExt.Code + ") has been " + msg.Action + "d";
                userprofile.activityLog(msg.Action, "<b>PhoneExt (" + phoneExt.Code + ")</b> has been " + msg.Action + "d", phoneExt.ID, "PhoneExt");
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(msg.Action, msg.MessageDetail, phoneExt.ID, "PhoneExt");
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
                List<FetchPhoneExt_Result> listPhoneExt = db.FetchPhoneExt(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, filters.Search, filters.Active).ToList();
                callBackData = listPhoneExt.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public ReturnData FetchPhoneExtById(int phoneExtID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                PhoneExt phoneExt = db.PhoneExts.AsNoTracking().FirstOrDefault(u => u.ID == phoneExtID);
                returnData.Data = phoneExt;
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
