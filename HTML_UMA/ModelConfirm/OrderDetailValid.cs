using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTML_UMA.ModelConfirm
{
    public class OrderDetailValid
    {
        [Required]
        public string detail_ShipName { get; set; }
        [Required]
        public string detail_ShipLastName { get; set; } 
        [Required]
        public string detail_ShipPhone { get; set; }
        [Required]
        public string detail_ShipEmail { get; set; }
        [Required]
        public string detail_ShipProvince { get; set; }
        [Required]
        public string detail_ShipDistrict { get; set; }
        [Required]
        public string detail_ShipTown { get; set; }
        [Required]
        public string detail_ShipStreet { get; set; }
        [Required]
        public string detail_PayName { get; set; }
        [Required]
        public string detail_PayLastName { get; set; }
        [Required]
        public string detail_PayPhone { get; set; }
        [Required]
        public string detail_PayEmail { get; set; }
        [Required]
        public string detail_PayProvince { get; set; }
        [Required]
        public string detail_PayDistrict { get; set; }
        [Required]
        public string detail_PayTown { get; set; }
        [Required]
        public string detail_PayStreet { get; set; }

    }
}