using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LocationLogic
    {
        public TriotechDbEntities db = new TriotechDbEntities();
        public Message Save(Location location, AspNetUser userprofile)
        {
            Message msg = new Message();
            try
            {
                if (location.ID != 0)
                {
                    msg.Action = "Update";
                    Location foundLocation = db.Locations.FirstOrDefault(x => x.ID == location.ID);
                    foundLocation.Name = location.Name;
                    foundLocation.IsActive = location.IsActive;
                    db.Entry(foundLocation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                else
                {
                    msg.Action = "Save";
                    db.Locations.Add(location);
                    db.SaveChanges();

                }
                msg.MessageDetail = "Location (" + location.Name + ") has been " + msg.Action + "d";
                userprofile.activityLog(msg.Action, "<b>Location (" + location.Name + ")</b> has been " + msg.Action + "d", location.ID, "Location");
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(msg.Action, msg.MessageDetail, location.ID, "Location");
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
                List<FetchLocation_Result> listLocation = db.FetchLocation(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, filters.Search, filters.Active).ToList();
                callBackData = listLocation.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public ReturnData FetchLocationById(int locationID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                Location product = db.Locations.AsNoTracking().FirstOrDefault(u => u.ID == locationID);
                returnData.Data = product;
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
