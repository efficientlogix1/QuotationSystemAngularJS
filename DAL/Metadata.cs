using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public partial class CommonFilters
    {
        public string Search { get; set; }
        public string Active { get; set; }
        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }
        public string SearchDateRange { get; set; }
    }
    public partial class AspNetUser : CommonFilters
    {
        public string Name
        {
            get
            {
                return (this.FirstName + " " + (String.IsNullOrEmpty(this.LastName) ? string.Empty : this.LastName));
            }
        }
        public string RoleName { get; set; }
        public string StrExpiryDate { get; set; }
    }
    public partial class BuyerRequest : CommonFilters
    {
        public string BuyerName { get; set; }
        public string StatusName { get; set; }
        public string ActionName { get; set; }
        //public int BuyerID { get; set; }
        //public int ActionID { get; set; }
    }

    
    public partial class FetchUser_Result 
    {
        public string StatusIsActive { get; set; }
    }
    public partial class Product:CommonFilters
    {
        public string CategoryName { get; set; }
    }
    public partial class Category : CommonFilters
    {
    }
    public partial class ProductRequest
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
    }
    public partial class OrderRequest
    {
        public int BuyerRequestID { get; set; }
        public string RequesterName { get; set; }
        public int StatusID { get; set; }
        public VendorRequest selectedVendeorRequest { get; set; }
        public string VendorName { get; set; }
        public string VendorCompany { get; set; }
        public string BuyerName { get; set; }
        public List<double> lstPrice { get; set; }
        public BuyerRequest selectedBuyer { get; set; }
        public List<FetchVendorRequestModel> lstVendors { get; set; }
        public int VendorRequestID { get; set; }
        public int BuyerActionID { get; set; }
    }
    public partial class FetchBuyerBusinessRecord_Result
    {
        public string StrBuyerPrice { get; set; }
    }
    public partial class FetchBuyerSentOrderRequests_Result
    {
        public bool IsSent { get; set; }
    }
    public partial class FetchVendorRequestModel
    {

        public int ID { get; set; }
        public string Name { get; set; }
    }
    public partial class VendorRequest
    {
        public string VendorName { get; set; }
        public string VendorCompany { get; set; }
    }
    public partial class OrderManagementRequest
    {
        public string StrRequestDate { get; set; }
        public string RequesterName { get; set; }
        public string SupervisorName { get; set; }
        public string ManagementName { get; set; }
        public string FinalApprovalName { get; set; }
    }
}
