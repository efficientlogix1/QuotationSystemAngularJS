using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL
{
    public class UserLogic
    {
        public TriotechDbEntities db = new TriotechDbEntities();
        //public Message UserProfileSetup(AspNetUser model, HttpPostedFileBase postedFile,string imagepath)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        AspNetUser user = db.AspNetUsers.FirstOrDefault(s => s.Id == model.Id);
        //        if (postedFile != null && postedFile.ContentLength > 0)
        //        {
        //            string extension = Path.GetExtension(postedFile.FileName).ToLower();
        //            string image = ".png,.jpg,.jpeg,.bmp";
        //            if (image.Contains(extension))
        //            {
        //                string path = imagepath;
        //                model.Photo = model.Id + "_" + path.FetchUniquePath(postedFile.FileName);
        //                if (!String.IsNullOrEmpty(model.Photo))
        //                {
        //                    if (System.IO.File.Exists(path + "/" + model.Photo))
        //                    {
        //                        System.IO.File.Delete(path + "/" + model.Photo);

        //                    }
        //                }
        //                postedFile.SaveAs(path + "/" + model.Photo);
        //            }

        //        }
        //        if (user != null)
        //        {
        //            msg.Action = "Save";
        //            if (user != null)
        //            {
        //                user.FirstName = model.FirstName;
        //                user.LastName = model.LastName;
        //                user.PhoneNumber = model.PhoneNumber;
        //                user.Email = model.Email;
        //                user.Address = model.Address;
        //                user.Country = model.Country;
        //                user.State = model.State;
        //                user.City = model.City;
        //                user.Street = model.Street;
        //                user.Near = model.Near;
        //                user.IsActive = user.IsActive;
        //                if (!string.IsNullOrEmpty(model.Photo))
        //                {
        //                    user.Photo = model.Photo;
        //                }

        //                //user.Description = model.Description;
        //                db.SaveChanges();
        //            }
        //        }



        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        public CallBackData Fetch(Paging paging, AspNetUser currentUser)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                AspNetUser user = paging.SearchJson.Deserialize<AspNetUser>();
                int userRoleID = db.AspNetRoles.FirstOrDefault(r => r.Name.ToLower() == user.RoleName.ToLower()).Id;
                List<FetchUser_Result> listUsers = db.FetchUser(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, currentUser.Id, userRoleID, user.Search, user.Active).ToList();
                foreach (var item in listUsers)
                {
                    if (item.ExpiryDate == null)
                    {
                        if (item.IsActive == true)
                        {
                            item.StatusIsActive = "Active";
                        }
                        else
                        {
                            item.StatusIsActive = "InActive";
                        }
                        item.IsEmailSent = true;
                    }
                    else
                    {
                        var currentDate = DateTime.Now.Date;
                        if (item.ExpiryDate.Value.Date < currentDate)
                        {
                            item.StatusIsActive = "Expired";
                            if (item.IsEmailSent == null || item.IsEmailSent.Value == false)
                            {
                                item.IsEmailSent = false;
                            }
                            else
                            {
                                item.IsEmailSent = true;
                            }
                        }
                        else
                        {
                            if (item.IsActive == true)
                            {
                                item.StatusIsActive = "Active";
                            }
                            else
                            {
                                item.StatusIsActive = "InActive";
                            }
                            item.IsEmailSent = true;
                        }
                    }
                }
                callBackData = listUsers.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public ReturnData FetchPrductById(int productID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                Product product = db.Products.AsNoTracking().FirstOrDefault(u => u.ID == productID);
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
