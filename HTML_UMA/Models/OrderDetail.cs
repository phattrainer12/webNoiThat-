//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HTML_UMA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public int detail_ID { get; set; }
        public Nullable<int> user_ID { get; set; }
        public Nullable<int> cart_ID { get; set; }
        public Nullable<int> ID { get; set; }
        public Nullable<decimal> detail_TotalBegin { get; set; }
        public Nullable<decimal> detail_fee { get; set; }
        public Nullable<decimal> detail_Totalmoney { get; set; }
        public string detail_ShipName { get; set; }
        public string detail_ShipLastName { get; set; }
        public string detail_ShipPhone { get; set; }
        public string detail_ShipEmail { get; set; }
        public string detail_ShipProvince { get; set; }
        public string detail_ShipDistrict { get; set; }
        public string detail_ShipTown { get; set; }
        public string detail_ShipStreet { get; set; }
        public string detail_PayName { get; set; }
        public string detail_PayLastName { get; set; }
        public string detail_PayPhone { get; set; }
        public string detail_PayEmail { get; set; }
        public string detail_PayProvince { get; set; }
        public string detail_PayDistrict { get; set; }
        public string detail_PayTown { get; set; }
        public string detail_PayStreet { get; set; }
        public Nullable<System.DateTime> detail_dateorder { get; set; }
        public string detail_note { get; set; }
        public Nullable<bool> @checked { get; set; }
        public string Paymethod { get; set; }
        public string apptransid { get; set; }
        public string statuspay { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual SaleCode SaleCode { get; set; }
        public virtual User User { get; set; }
        public virtual Notify Notify { get; set; }
    }
}
