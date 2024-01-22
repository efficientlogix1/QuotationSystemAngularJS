using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Triotech.Models;
using LinqToExcel;
using OfficeOpenXml;

namespace Triotech.Controllers
{

    [Authorize]
    public class CategoryController : Controller
    {
        Message msg = new Message();
        // GET: Category
        public ActionResult CategoryList()
        {
            return View();
        }
        public ActionResult CategorySetup()
        {
            return View();
        }
        public JsonResult Save(Category category, HttpPostedFileBase ggg)
        {

            return Json((new CategoryLogic()).Save(category, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchCategoryByID(int categoryID)
        {
            return Json((new CategoryLogic()).FetchCategoryById(categoryID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Fetch()
        {
            CallBackData callBackData = (new CategoryLogic()).Fetch(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }


        public FileResult GetExcel(string filename)
        {
            string ReportURL = Server.MapPath("~/Content/images/UploadExcelFile/BOM List"); //"{Your File Path}";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public JsonResult UploadExcelFIle()
        {
            List<string> lstcheckDuplicity = new List<string>();
            List<string> lstContainsDuplicatevalue = new List<string>();
            string WorksheetHeaderInfo = "";
            TriotechDbEntities dbc = new TriotechDbEntities();
            List<string> lstnotEnterCategory = new List<string>();
            var getCategoryData = dbc.Categories.ToList();

            var categoryList = new List<Category>();
            HttpPostedFileBase postedFile = Request.Files[0];
            var filename = postedFile.FileName;
            string targetpath = Server.MapPath("~/UploadExcelFile/");
            //    postedFile.SaveAs(targetpath + filename);
            //   string pathToExcelFile = targetpath + filename;
            string fileContentType = postedFile.ContentType;
            byte[] fileBytes = new byte[postedFile.ContentLength];
            var data = postedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(postedFile.ContentLength));


            using (var package = new ExcelPackage(postedFile.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {

                    var category = new Category();
                    category.Number = workSheet.Cells[rowIterator, 1].Value.ToString();
                    category.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                    category.Type = workSheet.Cells[rowIterator, 3].Value.ToString();
                    category.Description = workSheet.Cells[rowIterator, 4].Value.ToString();
                    var CategoryNumber = workSheet.Cells[1, 1].Text.ToString();
                    var CategoryName = workSheet.Cells[1, 2].Text.ToString();
                    var CategoryType = workSheet.Cells[1, 3].Text.ToString();
                    var CategoryDescription = workSheet.Cells[1, 4].Text.ToString();
                    if (CategoryNumber == "Category Number" && CategoryName == "Category Name" && CategoryType == "Category Type" && CategoryDescription == "Description")
                    {
                        var checkDuplicateNumber = lstcheckDuplicity.Where(x => x.Contains(category.Number)).FirstOrDefault();
                        var checkExistingNumber = getCategoryData.Where(x => x.Number == category.Number).FirstOrDefault().Number;
                        if (checkExistingNumber != null)
                        {
                            if (checkDuplicateNumber!=null)
                            {
                                
                                lstContainsDuplicatevalue.Add(category.Number);
                                break;
                            }

                            lstnotEnterCategory.Add(category.Number);
                            lstcheckDuplicity.Add(category.Number);
                        }

                        else
                        {
                            
                            if (checkDuplicateNumber=="")
                            {

                                lstcheckDuplicity.Add(category.Number);
                                categoryList.Add(category);
                            }
                            else
                            {
                                break; 
                            }

                        }

                    }
                    else
                    {
                        WorksheetHeaderInfo = "Please check the column Header Not Match with the Table record";
                        goto end;

                    }


                }
            }

            try
            {
                using (TriotechDbEntities db = new TriotechDbEntities())
                {
                    if (categoryList.Count != 0)
                    {
                        foreach (var item in categoryList)
                        {
                            item.IsActive = true;
                            item.CreationDate = System.DateTime.Now;
                            db.Categories.Add(item);


                        }
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }


            string ShowMessage = "";

        end:

            if (lstnotEnterCategory.Count == 0 && categoryList.Count != 0)
            {

                ShowMessage = "Successfully enter all the Category";
                msg.MessageDetail = ShowMessage;
            }
            else if (WorksheetHeaderInfo != "")
            {
                msg.MessageDetail = WorksheetHeaderInfo;
            }
            else if (categoryList.Count == 0 && lstnotEnterCategory.Count == 0)
            {

                ShowMessage = "No Record Exists in excel file";
                msg.Info = true;
                msg.MessageDetail = ShowMessage;


            }

            else if (categoryList.Count != 0 && lstnotEnterCategory.Count != 0)
            {
                ShowMessage = "Successfully Enter all the Category.Instead of all these.Because they are already existed.There List are as Follow" + lstnotEnterCategory;
                msg.Info = true;
                msg.MessageDetail = ShowMessage;
            }

            else if (lstnotEnterCategory.Count != 0 && categoryList.Count == 0)
            {
                ShowMessage = "All Category Number of Excel File already Exists in DataBase";
                msg.Info = true;
                msg.MessageDetail = ShowMessage;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
            //return Json((new CategoryLogic()).SaveCategoryDataByFile(package ,(new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
    }
}