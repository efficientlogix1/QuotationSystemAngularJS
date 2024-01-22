using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BLL
{
    public class VisitorLogic
    {
        public TriotechDbEntities db = new TriotechDbEntities();
        public CallBackData Fetch(Paging paging)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters visitor = paging.SearchJson.Deserialize<CommonFilters>();
                List<FetchVisitor_Result> listVisitor = db.FetchVisitor(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, visitor.Search).ToList();
                callBackData = listVisitor.ToDataTable(paging);


            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }

        public Message Save(Visitor visitor)
        {
            Message msg = new Message();
            try
            {
                msg.Action = "save";
                visitor.VisitTime = DateTime.UtcNow;
                db.Visitors.Add(visitor);
                db.SaveChanges();
                //userProfile.LogTransaction(msg.Action, msg.MessageDetail, userProfile.Id, "user");
            }
             
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                //userProfile.LogError(ex, "BLL/UserLogic/Save");
            }

            msg.MessageDetail = "Record has been saved";
            return msg;
        }

    }
}
