using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class ProductLogic
    {
        public TriotechDbEntities db = new TriotechDbEntities();

        /// <summary>
        /// save or update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Message Save(Product product, AspNetUser userprofile)
        {
            Message msg = new Message();
            try
            {
                if (product.ID != 0)
                {
                    msg.Action = "Update";
                    Product foundProduct = db.Products.FirstOrDefault(x => x.ID == product.ID);
                    foundProduct.Name = product.Name;
                    foundProduct.Code = product.Code;
                    foundProduct.Description = product.Description;
                    foundProduct.IsActive = product.IsActive;
                    foundProduct.CategoryID = product.CategoryID;
                    db.Entry(foundProduct).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                else
                {
                    msg.Action = "Save";
                    product.CreationDate = DateTime.UtcNow;
                    db.Products.Add(product);
                    db.SaveChanges();

                }
                msg.MessageDetail = "<b>Product (" + product.Name + ")</b> has been " + msg.Action + "d";
                userprofile.activityLog(msg.Action, msg.MessageDetail, product.ID, "Product");
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(msg.Action, msg.MessageDetail, product.ID, "Product");
                //userProfile.LogError(ex, "BLL/UserTypeLogic/Save");
            }

            return msg;

        }
        public CallBackData Fetch(Paging paging)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                Product product = paging.SearchJson.Deserialize<Product>();
                List<FetchProducts_Result> listProduct = db.FetchProducts(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, product.CategoryID, product.Search, product.Active).ToList();
                callBackData = listProduct.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        //public ReturnData FetchTableData()
        //{
        //    ReturnData returnData = new ReturnData();
        //    try
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        returnData.Data = db.Products.AsNoTracking().ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        returnData.msg.Success = false;
        //        returnData.msg.MessageDetail = Message.ErrorMessage;
        //    }

        //    return returnData;

        //}
        ////fetch one usertype for edit
        public ReturnData FetchProductById(int productID)
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
