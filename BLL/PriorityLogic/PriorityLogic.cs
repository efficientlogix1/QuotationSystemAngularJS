using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
  public  class PriorityLogic
    {

        public TriotechDbEntities db = new TriotechDbEntities();
        public Message Save(Priority priority, AspNetUser userprofile)
        {
            Message msg = new Message();
            try
            {
                if (priority.ID != 0)
                {
                    msg.Action = "Update";
                    Priority foundPriority = db.Priorities.FirstOrDefault(x => x.ID == priority.ID);
                    foundPriority.Name = priority.Name;
                    foundPriority.IsActive = priority.IsActive;
                    db.Entry(foundPriority).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                else
                {
                    msg.Action = "Save";
                    db.Priorities.Add(priority);
                    db.SaveChanges();

                }
                msg.MessageDetail = "Priority (" + priority.Name + ") has been " + msg.Action + "d";
                userprofile.activityLog(msg.Action, "<b>Priority (" + priority.Name + ")</b> has been " + msg.Action + "d", priority.ID, "Priority");
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(msg.Action, msg.MessageDetail, priority.ID, "Priority");
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
                List<FetchPriority_Result> listPriority = db.FetchPriority(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, filters.Search, filters.Active).ToList();
                callBackData = listPriority.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public ReturnData FetchPriorityById(int PriorityID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                Priority priority = db.Priorities.AsNoTracking().FirstOrDefault(u => u.ID == PriorityID);
                returnData.Data = priority;
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
