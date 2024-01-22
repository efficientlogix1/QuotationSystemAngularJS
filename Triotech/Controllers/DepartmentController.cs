using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Triotech.Models;

namespace Triotech.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult Save(Department department)
        {

            return Json((new DepartmentLogic()).Save(department, (new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchdepartmentByID(int departmentID)
        {
            return Json((new DepartmentLogic()).FetchDepartmentById(departmentID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Fetch()
        {
            CallBackData callBackData = (new DepartmentLogic()).Fetch(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }

    }
}