using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class RoleRequest
    {
        [Key]
        public int RoleRequestId { get; set; }

        //public string DesiredRole { get; set; }

        //Reason for request
        public string Reason { get; set; }

        [Display(Name = "Request Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RequestTime { get; set; }

        public rolesAvailable DesiredRole { get; set; }

        //One to One relationship with user
        [ForeignKey("User")]
        public string Id { get; set; }
        public User User { get; set; }
    }

    public enum rolesAvailable
    {
        Writer,
        Admin,
        Suspend//If they don't want their account to be usable anymore
    }
}