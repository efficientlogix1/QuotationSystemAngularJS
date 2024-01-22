//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class VendorRequest
    {
        public int ID { get; set; }
        public int VendorID { get; set; }
        public int BuyerRequestID { get; set; }
        public int StatusID { get; set; }
        public Nullable<double> Qoutation1 { get; set; }
        public Nullable<double> Qoutation2 { get; set; }
        public Nullable<double> Qoutation3 { get; set; }
        public string VendorDescription { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> QoutationDate { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual BuyerRequest BuyerRequest { get; set; }
        public virtual OrderRequestStatu OrderRequestStatu { get; set; }
    }
}
