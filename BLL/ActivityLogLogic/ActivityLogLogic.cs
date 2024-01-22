using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using DAL;

namespace BLL
{
   public   class ActivityLogLogic
    {
        private TriotechDbEntities db = new TriotechDbEntities();
        public CallBackData Fetch(Paging paging)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters  activityLog = paging.SearchJson.Deserialize<CommonFilters>();
               
                
                List<FetchActivityLog_Result> listActivityLog = db.FetchActivityLog(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder,
                     (String.IsNullOrEmpty(activityLog.SearchDateRange) ? null : activityLog.SearchDateRange.Split('-')[0].DbDate()),
                   (String.IsNullOrEmpty(activityLog.SearchDateRange) ? null : activityLog.SearchDateRange.Split('-')[1].DbDate()),
                    activityLog.Search).ToList();
                callBackData = listActivityLog.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
    }
}
