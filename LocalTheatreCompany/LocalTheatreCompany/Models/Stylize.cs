using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Stylize
    {
        [Key]
        public int StyleId { get; set; }

        public string BackGroundColour { get; set; }

        public string Font { get; set; }

        public bool IsApproved { get; set; }

        [Display(Name = "Request Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RequestTime { get; set; }


        //One to One relationship with user
        [ForeignKey("User")]
        public string Id { get; set; }
        public User User { get; set; }




    }
}