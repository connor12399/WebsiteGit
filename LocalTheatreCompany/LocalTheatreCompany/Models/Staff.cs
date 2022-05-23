using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Staff : User
    {
        [Display(Name = "Date Promoted")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DatePromoted { get; set; }

        //Navigation -Many
        public List<Blog> Blogs { get; set; }
    }
}