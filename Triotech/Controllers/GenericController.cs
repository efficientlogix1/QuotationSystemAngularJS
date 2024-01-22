using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace Triotech.Controllers
{
    [Authorize]
    public class GenericController : Controller
    {
        public JsonResult FetchCategories()
        {
            return Json((new Generic()).FetchCategories(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchProducts()
        {
            return Json((new Generic()).FetchProducts(), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult FetchCategoriesAndProducts()
        //{
        //    return Json((new Generic()).FetchCategoriesAndProducts(), JsonRequestBehavior.AllowGet);
        //}
    }
}