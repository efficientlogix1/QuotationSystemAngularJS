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
    [Authorize]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductList()
        {
            return View();
        }
        public ActionResult ProductSetup()
        {
            return View();
        }
        public JsonResult Save(Product product)
        {
            return Json((new ProductLogic()).Save(product,(new GenericModel()).FetchUserProfile()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchProductByID(int productID)
        {
            return Json((new ProductLogic()).FetchProductById(productID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Fetch(string roleName)
        {
            CallBackData callBackData = (new ProductLogic()).Fetch(Request.FetchPaging());
            return Json(callBackData, JsonRequestBehavior.AllowGet);
        }

    }
}