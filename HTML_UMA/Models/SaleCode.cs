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
    
    public partial class SaleCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaleCode()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int ID { get; set; }
        public string sale_code { get; set; }
        public Nullable<decimal> sale_price { get; set; }
        public Nullable<bool> used { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
