using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RequestsLogic
    {
        public TriotechDbEntities db = new TriotechDbEntities();
        public ReturnData SaveBuyerPendingOrder(BuyerRequest buyerRequest, string url, int statuID, AspNetUser userprofile)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                if (buyerRequest.ID != 0)
                {
                    returnData.msg.Action = "Update";
                    BuyerRequest foundBuyerRequest = db.BuyerRequests.FirstOrDefault(x => x.ID == buyerRequest.ID);

                    if (buyerRequest.ActionID == 4)
                    {
                        foundBuyerRequest.VendorRequests = null;
                        foundBuyerRequest.ActionID = buyerRequest.ActionID;
                        foundBuyerRequest.StatusID = buyerRequest.StatusID;
                        foundBuyerRequest.BuyerDescription = buyerRequest.BuyerDescription;
                        db.Entry(foundBuyerRequest).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        foreach (var item in buyerRequest.VendorRequests)
                        {
                            item.BuyerRequestID = buyerRequest.ID;
                        }
                        foundBuyerRequest.ActionID = buyerRequest.ActionID;
                        foundBuyerRequest.StatusID = buyerRequest.StatusID;
                        foundBuyerRequest.BuyerDescription = buyerRequest.BuyerDescription;
                        db.Entry(foundBuyerRequest).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        var lstVendorRequest = db.VendorRequests.AsNoTracking().Where(x => x.BuyerRequestID == buyerRequest.ID).ToList();
                        foreach (var item in lstVendorRequest)
                        {
                            if (item.ID != 0)
                            {
                                var vendorRequest = buyerRequest.VendorRequests.FirstOrDefault(i => i.ID == item.ID);
                                if (vendorRequest == null)
                                {
                                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                                }
                                else
                                {
                                    buyerRequest.StatusID = statuID;
                                    db.Entry(buyerRequest).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }

                        foreach (var item in buyerRequest.VendorRequests)
                        {
                            if (item.ID == 0)
                            {
                                item.BuyerRequestID = buyerRequest.ID;
                                item.StatusID = 3;
                                db.VendorRequests.Add(item);
                            }
                        }
                        db.SaveChanges();
                    }

                }
                returnData.msg.MessageDetail = "Request has been " + returnData.msg.Action + "d";
                userprofile.activityLog(returnData.msg.Action, "<b>Request</b> has been " + returnData.msg.Action + "d", buyerRequest.ID, "BuyerRequest & VendorRequest");
            }
            //catch (Exception ex)
            //{
            //    returnData.msg.Success = false;
            //    returnData.msg.MessageDetail = Message.ErrorMessage;
            //    //userProfile.LogError(ex, "BLL/UserTypeLogic/Save");
            //}
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            returnData.Data = url;
            return returnData;
        }
        public ReturnData SaveRequesterOrder(OrderRequest orderRequest, string url, AspNetUser userprofile)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                if (orderRequest.ID != 0)
                {
                    returnData.msg.Action = "Update";
                    OrderRequest foundOrderRequest = db.OrderRequests.FirstOrDefault(x => x.ID == orderRequest.ID);

                    foreach (var item in orderRequest.BuyerRequests)
                    {
                        item.RequestID = orderRequest.ID;
                    }
                    foreach (var item in orderRequest.ProductRequests)
                    {
                        item.RequestID = orderRequest.ID;
                    }
                    foundOrderRequest.Title = orderRequest.Title;
                    foundOrderRequest.ActionID = orderRequest.ActionID;
                    db.Entry(foundOrderRequest).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    //var lstBuyerRequest = db.BuyerRequests.AsNoTracking().Where(x => x.RequestID == orderRequest.ID).ToList();
                    //foreach (var item in lstBuyerRequest)
                    //{
                    //    if (item.ID != 0)
                    //    {
                    //        var buyerRequest = orderRequest.BuyerRequests.FirstOrDefault(i => i.ID == item.ID);
                    //        if (buyerRequest == null)
                    //        {
                    //            db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    //        }
                    //        else
                    //        {
                    //            if (buyerRequest.ID == orderRequest.BuyerRequestID)
                    //            {

                    //                buyerRequest.StatusID = statuID;
                    //                db.Entry(buyerRequest).State = System.Data.Entity.EntityState.Modified;

                    //            }

                    //        }
                    //    }
                    //}
                    var lstProductRequest = db.ProductRequests.AsNoTracking().Where(x => x.RequestID == orderRequest.ID).ToList();
                    foreach (var item in lstProductRequest)
                    {
                        if (item.ID != 0)
                        {
                            var productRequest = orderRequest.ProductRequests.FirstOrDefault(i => i.ID == item.ID);
                            if (productRequest == null)
                            {
                                db.Entry(productRequest).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }
                    }
                    foreach (var item in orderRequest.ProductRequests)
                    {
                        if (item.ID == 0)
                        {

                            item.RequestID = foundOrderRequest.ID;
                            db.ProductRequests.Add(item);
                        }
                    }
                    foreach (var item in orderRequest.BuyerRequests)
                    {
                        if (item.ID == 0)
                        {
                            item.RequestID = foundOrderRequest.ID;
                            item.StatusID = 3;
                            db.BuyerRequests.Add(item);
                        }
                    }
                    db.SaveChanges();
                }


                else
                {
                    returnData.msg.Action = "Save";
                    orderRequest.RequestDate = DateTime.UtcNow;
                    foreach (var item in orderRequest.BuyerRequests)
                    {
                        item.StatusID = 3;
                    }
                    db.OrderRequests.Add(orderRequest);
                    db.SaveChanges();
                }
                returnData.msg.MessageDetail = "Request (" + orderRequest.Title + ") has been " + returnData.msg.Action + "d";
                userprofile.activityLog(returnData.msg.Action, "<b>Request (" + orderRequest.Title + ")</b> has been " + returnData.msg.Action + "d", orderRequest.ID, "OrderRequest");
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(returnData.msg.Action, returnData.msg.MessageDetail, orderRequest.ID, "OrderRequest");
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
            //userprofile.activityLog(returnData.msg.Action, returnData.msg.MessageDetail, orderRequest.ID, "OrderRequest");
            returnData.Data = url;
            return returnData;
        }
        public ReturnData SaveBuyerRequest(BuyerRequest buyerRequest, int statusID, string url, AspNetUser userprofile)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                if (buyerRequest.ID != 0)
                {
                    returnData.msg.Action = "Update";
                    BuyerRequest foundOrderRequest = db.BuyerRequests.FirstOrDefault(x => x.ID == buyerRequest.ID);

                    foreach (var item in buyerRequest.VendorRequests)
                    {
                        item.BuyerRequestID = buyerRequest.ID;
                    }
                    foundOrderRequest.BuyerPrice = buyerRequest.BuyerPrice;
                    foundOrderRequest.BuyerDescription = buyerRequest.BuyerDescription;
                    foundOrderRequest.StatusID = buyerRequest.StatusID;
                    db.Entry(foundOrderRequest).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var lstVendorRequest = db.VendorRequests.AsNoTracking().Where(x => x.BuyerRequestID == buyerRequest.ID).ToList();
                    foreach (var item in lstVendorRequest)
                    {
                        if (item.ID != 0)
                        {
                            var vendorRequest = buyerRequest.VendorRequests.FirstOrDefault(i => i.ID == item.ID);
                            if (buyerRequest == null)
                            {
                                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                            }
                            else
                            {
                                if (buyerRequest.ID == vendorRequest.BuyerRequestID)
                                {

                                    buyerRequest.StatusID = statusID;
                                    db.Entry(buyerRequest).State = System.Data.Entity.EntityState.Modified;

                                }

                            }
                        }
                    }
                    foreach (var item in buyerRequest.VendorRequests)
                    {
                        if (item.ID == 0)
                        {

                            item.BuyerRequestID = foundOrderRequest.ID;
                            item.StatusID = 3;
                            db.VendorRequests.Add(item);
                        }
                    }
                    db.SaveChanges();
                }


                else
                {
                    returnData.msg.Action = "Save";
                    //orderRequest.RequestDate = DateTime.UtcNow;
                    foreach (var item in buyerRequest.VendorRequests)
                    {
                        item.StatusID = 3;
                    }
                    db.BuyerRequests.Add(buyerRequest);
                    db.SaveChanges();

                }
                returnData.msg.MessageDetail = "Request has been " + returnData.msg.Action + "d";
                userprofile.activityLog(returnData.msg.Action, "<b>Request</b> has been " + returnData.msg.Action + "d", buyerRequest.ID, "BuyerRequest");
            }
            //catch (Exception ex)
            //{
            //    returnData.msg.Success = false;
            //    returnData.msg.MessageDetail = Message.ErrorMessage;
            //    //userProfile.LogError(ex, "BLL/UserTypeLogic/Save");
            //}
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            returnData.Data = url;
            userprofile.activityLog(returnData.msg.Action, returnData.msg.MessageDetail, buyerRequest.ID, "BuyerRequest");
            return returnData;
        }
        public ReturnData SaveBuyerPrice(BuyerRequest buyerRequest, string url, AspNetUser userprofile)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                if (buyerRequest.ID != 0)
                {
                    returnData.msg.Action = "Update";
                    BuyerRequest foundBuyerRequest = db.BuyerRequests.FirstOrDefault(x => x.ID == buyerRequest.ID);
                    OrderRequest foundOrderRequest = db.OrderRequests.FirstOrDefault(x => x.ID == foundBuyerRequest.RequestID);
                    foundOrderRequest.StatusID = 2;
                    foundBuyerRequest.ActionID = buyerRequest.ActionID;
                    foundBuyerRequest.StatusID = 2;
                    foundBuyerRequest.BuyerPrice = buyerRequest.BuyerPrice;
                    foundBuyerRequest.QoutationDate = DateTime.Now;
                    db.Entry(foundBuyerRequest).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(foundOrderRequest).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    returnData.msg.MessageDetail = "Request has been " + returnData.msg.Action + "d";
                    userprofile.activityLog(returnData.msg.Action, "<b>Request</b> has been " + returnData.msg.Action + "d", buyerRequest.ID, "BuyerRequest");
                }
                else
                {
                    returnData.msg.Success = false;
                    returnData.msg.MessageDetail = "No data updated";
                }

            }
            //catch (Exception ex)
            //{
            //    returnData.msg.Success = false;
            //    returnData.msg.MessageDetail = Message.ErrorMessage;
            //    //userProfile.LogError(ex, "BLL/UserTypeLogic/Save");
            //}
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            returnData.Data = url;
            userprofile.activityLog(returnData.msg.Action, returnData.msg.MessageDetail, buyerRequest.ID, "BuyerRequest");
            return returnData;
        }
        public ReturnData SaveVendorRequest(VendorRequest vendorRequest, string url, AspNetUser userprofile)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                if (vendorRequest.ID != 0)
                {
                    returnData.msg.Action = "Update";
                    VendorRequest foundVendorRequest = db.VendorRequests.FirstOrDefault(x => x.ID == vendorRequest.ID);
                    if (url == "/OrderRequest/VendorRequestList")
                    {
                        foundVendorRequest.StatusID = 2;// vendorRequest.StatusID;
                    }
                    else
                    {
                        foundVendorRequest.StatusID = vendorRequest.StatusID;
                    }
                    foundVendorRequest.Qoutation1 = vendorRequest.Qoutation1;
                    //foundVendorRequest.Qoutation2 = vendorRequest.Qoutation2;
                    //foundVendorRequest.Qoutation3 = vendorRequest.Qoutation3;
                    foundVendorRequest.VendorDescription = vendorRequest.VendorDescription;
                    foundVendorRequest.Comment = vendorRequest.Comment;
                    foundVendorRequest.QoutationDate = DateTime.Now;
                    db.Entry(foundVendorRequest).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    returnData.msg.MessageDetail = "Request has been " + returnData.msg.Action + "d";
                    userprofile.activityLog(returnData.msg.Action, "<b>Request</b> has been " + returnData.msg.Action + "d", vendorRequest.ID, "VendorRequest");
                }
                else
                {
                    returnData.msg.Success = false;
                    returnData.msg.MessageDetail = "No data updated";
                    userprofile.activityLog(returnData.msg.Action, returnData.msg.MessageDetail, vendorRequest.ID, "VendorRequest");
                }

            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(returnData.msg.Action, returnData.msg.MessageDetail, vendorRequest.ID, "VendorRequest");
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
            returnData.Data = url;
            return returnData;
        }

        public CallBackData FetchAdminOrders(Paging paging)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                BuyerRequest request = paging.SearchJson.Deserialize<BuyerRequest>();
                List<FetchAdminOrderRequests_Result> listRequests = db.FetchAdminOrderRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, request.ActionID, request.StatusID, request.Search).ToList();
                callBackData = listRequests.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public CallBackData FetchBuyerPendingOrders(Paging paging, int buyerID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                BuyerRequest request = paging.SearchJson.Deserialize<BuyerRequest>();
                List<FetchBuyerPendingOrderRequests_Result> listRequests = db.FetchBuyerPendingOrderRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, request.ActionID, request.StatusID, buyerID, request.Search).ToList();
                callBackData = listRequests.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public CallBackData FetchBuyerSentOrders(Paging paging, int buyerID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                BuyerRequest request = paging.SearchJson.Deserialize<BuyerRequest>();
                List<FetchBuyerSentOrderRequests_Result> listRequests = db.FetchBuyerSentOrderRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, request.StatusID, buyerID, request.Search).ToList();
                listRequests.ForEach(x =>
                {

                    if (x.BuyerPrice > 0)
                    {
                        x.IsSent = true;
                    }
                    else
                    {
                        x.IsSent = false;
                    }
                });
                callBackData = listRequests.ToDataTable(paging);

            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public CallBackData FetchRequesterOrders(Paging paging, int requesterID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                BuyerRequest request = paging.SearchJson.Deserialize<BuyerRequest>();
                List<FetchRequesterOrderRequests_Result> listRequests = db.FetchRequesterOrderRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, request.ActionID, request.StatusID, requesterID, request.Search).ToList();
                callBackData = listRequests.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public CallBackData FetchVendorOrders(Paging paging, int vendorID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                BuyerRequest request = paging.SearchJson.Deserialize<BuyerRequest>();
                List<FetchVendorOrderRequests_Result> listRequests = db.FetchVendorOrderRequests(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, request.ActionID, request.StatusID, vendorID, request.Search).ToList();
                callBackData = listRequests.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        ////fetch one usertype for edit
        public ReturnData FetchRequestById(int requestID, bool isRequester)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                BuyerRequest foundBuyerRequest = db.BuyerRequests.FirstOrDefault(r => r.ID == requestID);
                OrderRequest orderRequest = db.OrderRequests.AsNoTracking().FirstOrDefault(u => u.ID == foundBuyerRequest.RequestID);
                List<BuyerRequest> lstBuyerRequest = db.BuyerRequests.AsNoTracking().Where(b => b.RequestID == orderRequest.ID).ToList();
                List<ProductRequest> lstProductRequest = db.ProductRequests.AsNoTracking().Where(p => p.RequestID == orderRequest.ID).ToList();
                List<Product> lstProducts = db.Products.AsNoTracking().ToList();
                List<Category> lstCategory = db.Categories.AsNoTracking().ToList();
                foreach (var item in lstProductRequest)
                {
                    Product product = lstProducts.FirstOrDefault(p => p.ID == item.ProductID);
                    Category category = lstCategory.FirstOrDefault(p => p.ID == product.CategoryID);
                    item.ProductName = product.Name;
                    item.CategoryName = category.Name;
                    item.CategoryID = product.CategoryID;

                }

                orderRequest.BuyerRequests = lstBuyerRequest;
                orderRequest.ProductRequests = lstProductRequest;
                orderRequest.selectedBuyer = foundBuyerRequest;
                orderRequest.BuyerName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == foundBuyerRequest.BuyerID).Name;
                orderRequest.RequesterName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == orderRequest.RequesterID).Name;
                if (!isRequester)
                {

                    orderRequest.BuyerRequestID = requestID;
                    orderRequest.StatusID = foundBuyerRequest.StatusID;

                }
                returnData.Data = orderRequest;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
                // Static.UserProfile.LogError(ex, "BLL/CashLogic/Fetch");
            }

            return returnData;

        }
        public ReturnData FetchVendorRequestById(int requestID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                //BuyerRequest foundBuyerRequest = db.BuyerRequests.AsNoTracking().FirstOrDefault(r => r.ID == requestID);
                VendorRequest foundVendorRequest = new VendorRequest();
                List<VendorRequest> lstVendorRequests = db.VendorRequests.AsNoTracking().Where(x => x.BuyerRequestID == requestID).ToList();
                foundVendorRequest = lstVendorRequests.FirstOrDefault();
                int lowestprice = 0;
                List<int> lstVendorIDs = new List<int>();
                bool hasPrice = false;
                foreach (var item in lstVendorRequests)
                {
                    lstVendorIDs.Add(item.VendorID);
                    if (item.Qoutation1 != null)
                    {
                        int currentprice = Convert.ToInt32(item.Qoutation1);
                        if (hasPrice == false)
                        {
                            lowestprice = currentprice;
                            hasPrice = true;
                        }
                        if (lowestprice >= currentprice)
                        {
                            foundVendorRequest = item;
                        }
                    }
                }
                //VendorRequest foundVendorRequest = db.VendorRequests.AsNoTracking().FirstOrDefault(r => r.ID == requestID);
                BuyerRequest foundBuyerRequest = db.BuyerRequests.AsNoTracking().FirstOrDefault(r => r.ID == foundVendorRequest.BuyerRequestID);
                OrderRequest orderRequest = db.OrderRequests.AsNoTracking().FirstOrDefault(u => u.ID == foundBuyerRequest.RequestID);
                List<BuyerRequest> lstBuyerRequest = db.BuyerRequests.AsNoTracking().Where(b => b.RequestID == orderRequest.ID).ToList();
                List<ProductRequest> lstProductRequest = db.ProductRequests.AsNoTracking().Where(p => p.RequestID == orderRequest.ID).ToList();
                List<Product> lstProducts = db.Products.AsNoTracking().ToList();
                List<Category> lstCategory = db.Categories.AsNoTracking().ToList();
                foreach (var item in lstProductRequest)
                {
                    Product product = lstProducts.FirstOrDefault(p => p.ID == item.ProductID);
                    Category category = lstCategory.FirstOrDefault(p => p.ID == product.CategoryID);
                    item.ProductName = product.Name;
                    item.CategoryName = category.Name;
                    item.CategoryID = product.CategoryID;

                }
                if (foundBuyerRequest.BuyerPrice == null)
                {
                    foundBuyerRequest.BuyerPrice = 0;
                }
                if (foundBuyerRequest.ActionID == null)
                {
                    foundBuyerRequest.ActionID = 0;
                }
                orderRequest.lstPrice = new List<double>();
                if (foundVendorRequest.Qoutation1 != null)
                {
                    orderRequest.lstPrice.Add(foundVendorRequest.Qoutation1.Value);
                    //orderRequest.lstPrice.Add(foundVendorRequest.Qoutation2.Value);
                    //orderRequest.lstPrice.Add(foundVendorRequest.Qoutation3.Value);
                }

                orderRequest.BuyerRequests = lstBuyerRequest;
                orderRequest.ProductRequests = lstProductRequest;
                orderRequest.selectedVendeorRequest = foundVendorRequest;
                var vendorInfo = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == foundVendorRequest.VendorID);
                orderRequest.VendorName = vendorInfo.Name;
                orderRequest.VendorCompany = vendorInfo.CompanyProfile;
                orderRequest.BuyerName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == foundBuyerRequest.BuyerID).Name;
                orderRequest.RequesterName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == orderRequest.RequesterID).Name;
                orderRequest.selectedBuyer = foundBuyerRequest;
                orderRequest.VendorRequestID = foundVendorRequest.ID;

                orderRequest.BuyerRequestID = foundVendorRequest.BuyerRequestID;
                orderRequest.StatusID = foundVendorRequest.StatusID;
                orderRequest.lstVendors = new List<FetchVendorRequestModel>();
                var lstVendorsData = db.AspNetUsers.AsNoTracking().Where(a => lstVendorIDs.Contains(a.Id)).ToList();
                foreach (var item in lstVendorRequests)
                {
                    FetchVendorRequestModel vendorRequestModel = new FetchVendorRequestModel();
                    var vendor = lstVendorsData.FirstOrDefault(x => x.Id == item.VendorID);
                    vendorRequestModel.ID = item.ID;
                    vendorRequestModel.Name = vendor.Name;
                    orderRequest.lstVendors.Add(vendorRequestModel);
                }
                if (foundBuyerRequest.ActionID != null)
                {
                    orderRequest.BuyerActionID = foundBuyerRequest.ActionID.Value;
                }
                else
                {
                    orderRequest.BuyerActionID = 0;
                }
                
                returnData.Data = orderRequest;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
                // Static.UserProfile.LogError(ex, "BLL/CashLogic/Fetch");
            }

            return returnData;

        }
        public ReturnData FetchRequestVendorById(int requestID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                VendorRequest foundVendorRequest = db.VendorRequests.AsNoTracking().FirstOrDefault(r => r.ID == requestID);
                BuyerRequest foundBuyerRequest = db.BuyerRequests.AsNoTracking().FirstOrDefault(r => r.ID == foundVendorRequest.BuyerRequestID);
                OrderRequest orderRequest = db.OrderRequests.AsNoTracking().FirstOrDefault(u => u.ID == foundBuyerRequest.RequestID);
                List<BuyerRequest> lstBuyerRequest = db.BuyerRequests.AsNoTracking().Where(b => b.RequestID == orderRequest.ID).ToList();
                List<ProductRequest> lstProductRequest = db.ProductRequests.AsNoTracking().Where(p => p.RequestID == orderRequest.ID).ToList();
                List<Product> lstProducts = db.Products.AsNoTracking().ToList();
                List<Category> lstCategory = db.Categories.AsNoTracking().ToList();
                foreach (var item in lstProductRequest)
                {
                    Product product = lstProducts.FirstOrDefault(p => p.ID == item.ProductID);
                    Category category = lstCategory.FirstOrDefault(p => p.ID == product.CategoryID);
                    item.ProductName = product.Name;
                    item.CategoryName = category.Name;
                    item.CategoryID = product.CategoryID;

                }
                if (foundBuyerRequest.BuyerPrice == null)
                {
                    foundBuyerRequest.BuyerPrice = 0;
                }
                if (foundBuyerRequest.ActionID == null)
                {
                    foundBuyerRequest.ActionID = 0;
                }
                orderRequest.lstPrice = new List<double>();
                if (foundVendorRequest.Qoutation1 != null)
                {
                    orderRequest.lstPrice.Add(foundVendorRequest.Qoutation1.Value);
                    //orderRequest.lstPrice.Add(foundVendorRequest.Qoutation2.Value);
                    //orderRequest.lstPrice.Add(foundVendorRequest.Qoutation3.Value);
                }

                orderRequest.BuyerRequests = lstBuyerRequest;
                orderRequest.ProductRequests = lstProductRequest;
                orderRequest.selectedVendeorRequest = foundVendorRequest;
                orderRequest.VendorName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == foundVendorRequest.VendorID).Name;
                orderRequest.BuyerName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == foundBuyerRequest.BuyerID).Name;
                orderRequest.RequesterName = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == orderRequest.RequesterID).Name;
                orderRequest.selectedBuyer = foundBuyerRequest;
                orderRequest.VendorRequestID = foundVendorRequest.ID;

                orderRequest.BuyerRequestID = foundVendorRequest.BuyerRequestID;
                orderRequest.StatusID = foundVendorRequest.StatusID;
                orderRequest.lstVendors = new List<FetchVendorRequestModel>();
                returnData.Data = orderRequest;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
                // Static.UserProfile.LogError(ex, "BLL/CashLogic/Fetch");
            }

            return returnData;

        }
        public ReturnData PreBindBuyerRequest()
        {
            ReturnData returnData = new ReturnData();
            try
            {

                var lstReturn = new
                {
                    lstProducts = (new Generic()).FetchProducts(),
                    lstCategories = (new Generic()).FetchCategories(),
                    lstVendors = (new Generic()).FetchUserByRoleName("Vendor"),
                    lstActions = (new Generic()).FetchActions(false, true, false),
                    lstStatus = (new Generic()).FetchStatus()
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
        public ReturnData PreBindVendorRequest()
        {
            ReturnData returnData = new ReturnData();
            try
            {

                var lstReturn = new
                {
                    lstProducts = (new Generic()).FetchProducts(),
                    lstCategories = (new Generic()).FetchCategories(),
                    lstActions = (new Generic()).FetchActions(false, false, true),
                    lstStatus = (new Generic()).FetchStatus()
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
        public ReturnData PrebindRequesterOrderSetup()
        {
            ReturnData returnData = new ReturnData();
            try
            {

                var lstReturn = new
                {
                    lstProducts = (new Generic()).FetchProducts(),
                    lstCategories = (new Generic()).FetchCategories(),
                    lstBuyers = (new Generic()).FetchUserByRoleName("Buyer"),
                    lstActions = (new Generic()).FetchActions(true, false, false),
                    lstStatus = (new Generic()).FetchStatus()
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
        public ReturnData PreBindRequestList()
        {
            ReturnData returnData = new ReturnData();
            try
            {

                var lstReturn = new
                {
                    lstActions = (new Generic()).FetchActions(true, false, false),
                    lstStatus = (new Generic()).FetchStatus()
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
        public ReturnData FetchVendorRequestByVendorRequestId(int requestID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                VendorRequest foundVendorRequest = db.VendorRequests.FirstOrDefault(r => r.ID == requestID);
                var vendorInfo = db.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == foundVendorRequest.VendorID);
                foundVendorRequest.VendorName = vendorInfo.Name;
                foundVendorRequest.VendorCompany = vendorInfo.CompanyProfile;
                returnData.Data = foundVendorRequest;
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
