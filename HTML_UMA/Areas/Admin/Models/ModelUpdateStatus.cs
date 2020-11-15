using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTML_UMA.Areas.Admin.Models
{
    public class ModelUpdateStatus
    {
        public int Id { get; set; }
        public Boolean fastProduct { get; set; }
        public Boolean newProduct { get; set; }
        public Boolean sale { get; set; }
    }
}