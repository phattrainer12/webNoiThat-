using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTML_UMA.ModelConfirm
{
    public class productmodel
    {
        public int pro_ID { get; set; }
        public string pro_code { get; set; }
        public int Menu_ID { get; set; }
        public int group_ID { get; set; }
        public string pro_Name { get; set; }
        public decimal pro_price { get; set; }
        public int pro_Quanty { get; set; }
        public string pro_Description { get; set; }
        public int pro_View { get; set; }
        public string pro_Image { get; set; }
        public System.DateTime pro_Date { get; set; }
        public string pro_Materials { get; set; }
        public int Color_ID { get; set; }
        public string pro_Size { get; set; }
        public string pro_Designer { get; set; }
        public string pro_Source { get; set; }
        public string pro_Instruction { get; set; }
        public string pro_Status { get; set; }
        public string note { get; set; }
        public bool removeproduct { get; set; }
    }
}