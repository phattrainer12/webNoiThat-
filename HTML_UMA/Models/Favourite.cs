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
    
    public partial class Favourite
    {
        public int favourite_ID { get; set; }
        public Nullable<int> pro_ID { get; set; }
        public Nullable<int> user_ID { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}