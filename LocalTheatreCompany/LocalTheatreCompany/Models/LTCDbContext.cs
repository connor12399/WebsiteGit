using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LocalTheatreCompany.Models
{
    //Local Theatre Company Database Context
    public class LTCDbContext : IdentityDbContext<User>
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RoleRequest> RoleRequests { get; set; }
        public DbSet<Stylize> Stylizes { get; set; }


        public LTCDbContext()
            : base("LTCConnection2", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        public static LTCDbContext Create()
        {
            return new LTCDbContext();
        }
        
    }
}