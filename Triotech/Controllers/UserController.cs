using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Triotech.Models;
using BLL;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Triotech.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public TriotechDbEntities db = new TriotechDbEntities();
        // GET: User
        public ActionResult AdminList()
        {
            return View();
        }
        public ActionResult AdminSetup()
        {
            return View();
        }
        public JsonResult RegisterAdmin(RegisterViewModel model)
        {
            model.RoleId = 1;
            return Json(RegisterUser(model), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserProfile(int userId)
        {

            if (!User.IsInRole("Admin"))
            {
                AspNetUser currentUser = (new GenericModel()).FetchUserProfile();
                if (userId != currentUser.Id)
                {
                    userId = currentUser.Id;
                }
            }
            AspNetUser model = db.AspNetUsers.AsNoTracking().FirstOrDefault(u => u.Id == userId);
            string path = Server.MapPath("~/Content/images/ProfilePicture");
            if (String.IsNullOrEmpty(model.Photo))
            {
                model.Photo = "dummy-avatar.png";
            }
            else
            {
                if (!System.IO.File.Exists(path + "/" + model.Photo))
                {

                    model.Photo = "dummy-avatar.png";
                }
            }
            if (User.IsInRole("Admin"))
            {
                model.StrExpiryDate = "";
                if (model.ExpiryDate != null)
                {
                    model.StrExpiryDate = model.ExpiryDate.Value.ToString("dd-MM-yyyy");
                }
            }
            return View(model);
        }

        [HttpPost]
        //HttpPostedFileBase postedFile)
        public ActionResult UserProfile(AspNetUser model)
        {
            AspNetUser user = db.AspNetUsers.FirstOrDefault(s => s.Id == model.Id);
            //if (postedFile != null && postedFile.ContentLength > 0)
            //{
            //    string extension = Path.GetExtension(postedFile.FileName).ToLower();
            //    string image = ".png,.jpg,.jpeg";
            //    if (image.Contains(extension))
            //    {
            //        string path = Server.MapPath("~/Content/images/ProfilePicture");
            //        model.Photo = model.Id + "_" + path.FetchUniquePath(postedFile.FileName);
            //        if (!String.IsNullOrEmpty(model.Photo))
            //        {
            //            if (System.IO.File.Exists(path + "/" + model.Photo))
            //            {
            //                System.IO.File.Delete(path + "/" + model.Photo);

            //            }
            //        }
            //        postedFile.SaveAs(path + "/" + model.Photo);
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "Only png, jpg and jpeg formats are allowed");
            //        return View(model);
            //    }
            //}
            if (Request.Files != null)
            {
                HttpFileCollectionBase files = Request.Files;
                foreach (string file in files)
                {
                    try
                    {
                        HttpPostedFileBase postedFile = files[file];
                        string extension = Path.GetExtension(postedFile.FileName).ToLower();
                        string image = ".png,.jpg,.jpeg";
                        string pdfFile = ".pdf";
                        if (extension != string.Empty)
                        {
                            if (image.Contains(extension))
                            {
                                string path = Server.MapPath("~/Content/images/ProfilePicture");
                                if (!String.IsNullOrEmpty(model.Photo))
                                {
                                    if (System.IO.File.Exists(path + "/" + model.Photo))
                                    {
                                        System.IO.File.Delete(path + "/" + model.Photo);

                                    }
                                }
                                model.Photo = model.Id + "_" + path.FetchUniquePath(postedFile.FileName);
                                postedFile.SaveAs(path + "/" + model.Photo);
                            }
                            else if (pdfFile == extension)
                            {
                                string path = Server.MapPath("~/Content/images/VendorNDAFile");
                                if (!String.IsNullOrEmpty(model.NDAFile))
                                {
                                    if (System.IO.File.Exists(path + "/" + model.NDAFile))
                                    {
                                        System.IO.File.Delete(path + "/" + model.NDAFile);

                                    }
                                }
                                model.NDAFile = model.Id + "_" + path.FetchUniquePath(postedFile.FileName);
                                postedFile.SaveAs(path + "/" + model.NDAFile);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Only png, jpg and jpeg formats for profile and pdf for NDA are allowed");
                                return View(model);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        return View(model);
                    }
                }
            }
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.Country = model.Country;
                user.State = model.State;
                user.City = model.City;
                user.Street = model.Street;
                user.Near = model.Near;
                if (user.RoleId == 2)
                {
                    user.CompanyRegisteration = model.CompanyRegisteration;
                    user.CompanyProfile = model.CompanyProfile;
                }
                if (!string.IsNullOrEmpty(model.Photo))
                {
                    user.Photo = model.Photo;
                }
                if (!string.IsNullOrEmpty(model.NDAFile))
                {
                    user.NDAFile = model.NDAFile;
                }
                if (User.IsInRole("Admin") && User.Identity.GetUserId<int>() != user.Id)
                {
                    user.IsActive = model.IsActive;
                    user.PaymentTerms = model.PaymentTerms;
                    if (user.RoleId != 1)
                    {
                        if (model.StrExpiryDate != "")
                        {
                            user.ExpiryDate = DateTime.ParseExact(model.StrExpiryDate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            user.ExpiryDate = model.ExpiryDate;
                        }
                        user.IsEmailSent = false;
                    }
                }
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            //ViewBag.UpdateProfile = "Profile has been updated";
            TempData["SuccessMessage"] = "Profile has been updated";
            (new GenericModel()).FetchUserProfile().activityLog("Profile Update", "<b>Profile of '" + user.Name+ "'</b> been updated", user.Id, "AspNetUser");
            if (User.IsInRole("Admin") && User.Identity.GetUserId<int>() != user.Id)
            {
                if (user.RoleId == 1)
                {
                    return RedirectToAction("AdminList");
                }
                if (user.RoleId == 2)
                {
                    return RedirectToAction("VendorList");
                }
                if (user.RoleId == 3)
                {
                    return RedirectToAction("BuyerList");
                }
                if (user.RoleId == 4)
                {
                    return RedirectToAction("RequesterList");
                }
                if (user.RoleId == 5)
                {
                    return RedirectToAction("SupervisorList");
                }
                if(user.RoleId == 6)
                {
                    return RedirectToAction("ManagementList");
                }
               else
                {
                    return RedirectToAction("FinalApprovalList");
                }
            }
            else
            {

                return RedirectToAction("Index", "Home");
            }
        }

        //public JsonResult UserProfileSetup(AspNetUser model, HttpPostedFileBase postedFile)
        //{
        //    string path = Server.MapPath("~/Content/images/ProfilePicture");

        //    return Json((new UserLogic()).UserProfileSetup(model, postedFile, path), JsonRequestBehavior.AllowGet);
        //}




        public ActionResult VendorList()
        {
            return View();
        }
        public ActionResult VendorSetup()
        {
            return View();
        }
        public JsonResult RegisterVendor(RegisterViewModel model)
        {
            model.RoleId = 2;
            return Json(RegisterUser(model), JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuyerList()
        {
            return View();
        }
        public ActionResult BuyerSetup()
        {
            return View();
        }
        public JsonResult RegisterBuyer(RegisterViewModel model)
        {
            model.RoleId = 3;
            return Json(RegisterUser(model), JsonRequestBehavior.AllowGet);
        }
        public ActionResult RequesterList()
        {
            return View();
        }
        public ActionResult RequesterSetup()
        {
            return View();
        }
        public JsonResult RegisterRequester(RegisterViewModel model)
        {
            model.RoleId = 4;
            return Json(RegisterUser(model), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupervisorList()
        {
            return View();
        }
        public ActionResult SupervisorSetup()
        {
            return View();
        }

       

        public JsonResult RegisterSupervisor(RegisterViewModel model)
        {
            model.RoleId = 5;
            return Json(RegisterUser(model), JsonRequestBehavior.AllowGet);

        }


        public ActionResult FinalApprovalList()
        {
            return View();
        }
        public ActionResult FinalApprovalSetup()
        {
            return View();
        }

        public JsonResult RegisterFinalApproval(RegisterViewModel model)
        {
            model.RoleId = 7;
            return Json(RegisterUser(model), JsonRequestBehavior.AllowGet);

        }

        public ActionResult ManagementList()
        {
            return View();
        }
        public ActionResult ManagementSetup()
        {
            return View();
        }
        public JsonResult RegisterManagement(RegisterViewModel model)
        {
            model.RoleId = 6;
            return Json(RegisterUser(model), JsonRequestBehavior.AllowGet);
        }
        public Message RegisterUser(RegisterViewModel model)
        {
            Message msg = new Message();
            msg.Action = "Save";
            if (ModelState.IsValid)
            {
                if (model.UserId != 1)
                {
                    model.ExpiryDate = DateTime.Now.AddDays(30);
                }
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, Address = model.Address, FirstName = model.FirstName, LastName = model.LastName, CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now, IsActive = model.IsActive, RoleId = model.RoleId, PhoneNumber = model.PhoneNumber, Country = model.Country, State = model.State, City = model.City, Street = model.Street, Near = model.Near, PostalCode = model.PostalCode, PaymentTerms = model.PaymentTerms, ExpiryDate = model.ExpiryDate, CompanyRegisteration = model.CompanyRegisteration, CompanyProfile = model.CompanyProfile };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    var foundUserRole = db.AspNetRoles.AsNoTracking().FirstOrDefault(u => u.Id == model.RoleId);
                    UserManager.AddToRole(user.Id, foundUserRole.Name);
                    msg.MessageDetail = "New user has been saved successfully";
                    string profileName = user.FirstName + " " + (String.IsNullOrEmpty(user.LastName) ? string.Empty : user.LastName);
                    (new GenericModel()).FetchUserProfile().activityLog(foundUserRole.Name + " creation", "<b>Profile of '"+ profileName + "'</b> has been created", user.Id, "AspNetUser");
                    return msg;
                }
                msg.Success = false;
                foreach (var error in result.Errors)
                {
                    msg.MessageDetail += error + "\n";
                }
                (new GenericModel()).FetchUserProfile().activityLog(db.AspNetRoles.AsNoTracking().FirstOrDefault(u => u.Id == model.RoleId).Name + " creation", "<b>Profile creation</b> has been failed", 0, "AspNetUser");
                return msg;
            }
            else
            {
                string errorstring = string.Empty;
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errorstring += error.ErrorMessage+"\n";
                    }
                }
                msg.Success = false;
                (new GenericModel()).FetchUserProfile().activityLog(db.AspNetRoles.AsNoTracking().FirstOrDefault(u => u.Id == model.RoleId).Name + " creation", "<b>Profile creation</b> has been failed due to "+errorstring, 0, "AspNetUser");
                msg.MessageDetail = errorstring;
                return msg;
            }
        }

        public JsonResult Fetch()
        {
            CallBackData callBackData = (new UserLogic()).Fetch(Request.FetchPaging(), (new GenericModel()).FetchUserProfile());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }
        public FileResult GetPdf(string filename)
        {
            string ReportURL = Server.MapPath("~/Content/images/VendorNDAFile/" + filename); //"{Your File Path}";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }
        public JsonResult SendEmailToExpiredUser(int userId)
        {
            var user = db.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            Message msg = new Message();
            if (SendEmailtoUser(user))
            {
                user.IsEmailSent = true;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                msg.Action = "Email Sent";
                msg.MessageDetail = "Email has been sent to Buyer(" + user.Name + ")";
            }
            else
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        private bool SendEmailtoUser(AspNetUser model)
        {
            try
            {
                EmailTemplate emailTemplate = db.EmailTemplates.AsEnumerable().FirstOrDefault(x => x.Subject == "Renew Credentials");
                emailTemplate.To = string.Join(",", emailTemplate.To, model.Email);
                emailTemplate.MessageBody = emailTemplate.MessageBody.Replace("{#!Name!#}", model.Name);
                emailTemplate.SendEmail();
                emailTemplate.To = "";
                emailTemplate.CC = "";
                emailTemplate.BCC = "";
                emailTemplate.MessageBody = emailTemplate.MessageBody.Replace(model.Name, "{#!Name!#}");
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}