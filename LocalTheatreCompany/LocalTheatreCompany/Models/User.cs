using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LocalTheatreCompany.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {

        //********************************************8
        //Extending the identity user with folloiwng property
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [Display(Name = "Registered At")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegisteredAt { get; set; }


        public bool IsActive { get; set; }

        public bool IsSuspended { get; set; }

        //One to ONe
        public int StyleId { get; set; }

        //Nav Property - Many
        public List<Comment> Comments { get; set; }

        //One to One relationship
        //[ForeignKey("RoleRequest")]
        //public string RoleRequestId { get; set; }
        //public RoleRequest RoleRequest { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

}