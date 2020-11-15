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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int cart_ID { get; set; }
        public Nullable<int> pro_ID { get; set; }
        public Nullable<int> user_ID { get; set; }
        public Nullable<int> detail_ID { get; set; }
        public Nullable<int> cart_Quanty { get; set; }
        public Nullable<decimal> cart_TotalPrice { get; set; }
        public Nullable<decimal> pro_Price { get; set; }
        public string pro_Name { get; set; }
        public string pro_Description { get; set; }
        public Nullable<int> pro_View { get; set; }
        public string pro_Image { get; set; }
        public Nullable<System.DateTime> pro_Date { get; set; }
        public string pro_Materials { get; set; }
        public Nullable<int> Color_ID { get; set; }
        public string pro_Size { get; set; }
        public string pro_Designer { get; set; }
        public string pro_Source { get; set; }
        public string pro_Instruction { get; set; }
        public Nullable<bool> paid { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}