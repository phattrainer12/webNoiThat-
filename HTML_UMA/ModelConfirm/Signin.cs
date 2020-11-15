using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTML_UMA.ModelConfirm
{
    public class Signin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PassWord { get; set; }
        public DateTime DateCreate { get; set; }
        [Required]
        public bool Accept { get; set; }
    }
}