using DAL;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel;
using OfficeOpenXml;

namespace BLL
{
    public class CategoryLogic
    {
        public TriotechDbEntities db = new TriotechDbEntities();
        public Message Save(Category category,AspNetUser userprofile)
        {
            Message msg = new Message();
            try
            {
                if (category.ID != 0)
                {
                    msg.Action = "Update";
                    Category foundCategory = db.Categories.FirstOrDefault(x => x.ID == category.ID);
                    foundCategory.Name = category.Name;
                    foundCategory.Type = category.Type;
                    foundCategory.Number = category.Number;
                    foundCategory.Description = category.Description;
                    foundCategory.IsActive = category.IsActive;
                    db.Entry(foundCategory).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                else
                {
                    msg.Action = "Save";
                    category.CreationDate = DateTime.UtcNow;
                    db.Categories.Add(category);
                    db.SaveChanges();

                }
                msg.MessageDetail = "Category (" + category.Name + ") has been " + msg.Action + "d";
                userprofile.activityLog(msg.Action, "<b>Category (" + category.Name + ")</b> has been " + msg.Action + "d", category.ID, "Category");
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(msg.Action, msg.MessageDetail, category.ID, "Category");
                //userProfile.LogError(ex, "BLL/UserTypeLogic/Save");
            }

            return msg;
        }
        public CallBackData Fetch(Paging paging)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                Category category = paging.SearchJson.Deserialize<Category>();
                List<FetchCategories_Result> listCategory = db.FetchCategories(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, category.Search, category.Active).ToList();
                callBackData = listCategory.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }

        public ReturnData FetchCategoryById(int categoryID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                Category product = db.Categories.AsNoTracking().FirstOrDefault(u => u.ID == categoryID);
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
