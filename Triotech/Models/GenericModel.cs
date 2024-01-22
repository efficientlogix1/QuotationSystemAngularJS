using BLL;
using DAL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Triotech.Models
{
    public class GenericModel
    {
        public AspNetUser FetchUserProfile()
        {
            return System.Web.HttpContext.Current.User.Identity.GetUserId<int>().FetchUserProfile();
        }
    }
}