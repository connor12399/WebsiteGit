using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    //Will always drop the table data meaning only seaded data will appear upon restart
    public class DatabaseInitializer : DropCreateDatabaseAlways<LTCDbContext>
    {
        protected override void Seed(LTCDbContext context)
        {
            if (!context.Users.Any())
            {



                //to create the store roles we need a rolemanage
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //if the admin role doesn't exist
                if (!roleManager.RoleExists("Admin"))
                {
                    //then we create one
                    roleManager.Create(new IdentityRole("Admin"));
                }

                //if the admin role doesn't exist
                if (!roleManager.RoleExists("Writer"))
                {
                    //then we create one
                    roleManager.Create(new IdentityRole("Writer"));
                }

                //if the admin role doesn't exist
                if (!roleManager.RoleExists("Super-Admin"))
                {
                    //then we create one
                    roleManager.Create(new IdentityRole("Super-Admin"));
                }

                //if the admin role doesn't exist
                if (!roleManager.RoleExists("Member"))
                {
                    //then we create one
                    roleManager.Create(new IdentityRole("Member"));
                }

                //if the admin role doesn't exist
                if (!roleManager.RoleExists("Suspended"))
                {
                    //then we create one
                    roleManager.Create(new IdentityRole("Suspended"));
                }

                context.SaveChanges();



                //****************************CREATE USERS**************

                //to create users-customer or employees- we need UserManager
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                //Create an ADMIN
                //first check if the admin exists in the database


                // Super liberal password validation for password for seeds
                userManager.PasswordValidator = new PasswordValidator
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequireLowercase = false,
                    RequireNonLetterOrDigit = false,
                    RequireUppercase = false,
                };

                //create and  admin employee
                var administrator = new Staff
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Adam",
                    LastName = "Adamson",
                    Postcode = "G5 7GG",
                    RegisteredAt = DateTime.Now.AddYears(-5),
                    DatePromoted = DateTime.Now.AddYears(-2),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false,
                };

                //add admin to the users table
                userManager.Create(administrator, "admin123");

                //assign it to the Admin role
                userManager.AddToRole(administrator.Id, "Super-Admin");



                //if the doesn't then create him
                var jeff = new Staff
                {
                    UserName = "jeff@gmail.com",
                    Email = "jeff@gmail.com",
                    FirstName = "Jeff",
                    LastName = "Jefferson",
                    Postcode = "G9 7GG",
                    RegisteredAt = DateTime.Now.AddYears(-2),
                    DatePromoted = DateTime.Now.AddYears(-1),

                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false,
                };
                //aadd jeff and the password to the Users table
                userManager.Create(jeff, "admin123");

                //assign the Manager role to jeff
                userManager.AddToRole(jeff.Id, "Admin");


                var alex = new Staff
                {
                    UserName = "xander@gmail.com",
                    Email = "xander@gmail.com",
                    FirstName = "Alex",
                    LastName = "Alexei",
                    Postcode = "G3 7GG",
                    RegisteredAt = DateTime.Now.AddYears(-3),
                    DatePromoted = DateTime.Now.AddYears(-2),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false,
                };
                //aadd jeff and the password to the Users table
                userManager.Create(alex, "writerguy");

                //assign the Manager role to jeff
                userManager.AddToRole(alex.Id, "Writer");


                var customer = new User
                {
                    UserName = "bill@gmail.com",
                    Email = "bill@gmail.com",
                    FirstName = "Billy",
                    LastName = "Crow",
                    Postcode = "G56 7DF",
                    RegisteredAt = DateTime.Now.AddYears(-5),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false,
                };
                //aadd jeff and the password to the customer table
                userManager.Create(customer, "customer1");

                //assign the Manager role to 
                userManager.AddToRole(customer.Id, "Member");


                var bob = new User
                {
                    UserName = "bob@gmail.com",
                    Email = "bob@gmail.com",
                    RegisteredAt = DateTime.Now.AddYears(-1),
                    EmailConfirmed = true,
                    FirstName = "Bob",
                    LastName = "Williams",
                    Postcode = "G56 7DF",
                    IsActive = true,
                    IsSuspended = false,
                };
                //aadd jeff and the password to the customer table
                userManager.Create(bob, "customer2");

                //assign the Manager role to 
                userManager.AddToRole(bob.Id, "Member");


                var steve = new User
                {
                    UserName = "steveb@gmail.com",
                    Email = "steveb@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Steve",
                    LastName = "Fist",
                    Postcode = "EH6 7DF",
                    IsActive = true,
                    IsSuspended = false,
                    RegisteredAt = DateTime.Now.AddYears(-10),
                };
                //aadd jeff and the password to the customer table
                userManager.Create(steve, "customer3");

                //assign the Manager role to 
                userManager.AddToRole(steve.Id, "Member");


                //suspended user
                var gary = new User
                {
                    UserName = "gary@gmail.com",
                    Email = "gary@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Garry",
                    LastName = "Hugh",
                    Postcode = "G6 7DF",
                    IsActive = false,
                    IsSuspended = true,
                    RegisteredAt = DateTime.Now,
                };
                //aadd jeff and the password to the customer table
                userManager.Create(gary, "customer4");

                //assign the Manager role to 
                userManager.AddToRole(gary.Id, "Suspended");



                //save changes to database
                context.SaveChanges();

                ////////////////////////////////////////////////////////////////////////////////////
                //Seeding the Categories table//
                ////////////////////////////////////////////////////////////////////////////////////

                //create categories
                var cat1 = new Category() { CategoryName = "News" };
                var cat2 = new Category() { CategoryName = "Sport" };
                var cat3 = new Category() { CategoryName = "Health" };
                var cat4 = new Category() { CategoryName = "Weather" };

                //add all the categories
                context.Categories.Add(cat1);
                context.Categories.Add(cat2);
                context.Categories.Add(cat3);
                context.Categories.Add(cat4);

                //save changes to database
                context.SaveChanges();


                ////////////////////////////////////////////////////////////////////////////////////
                //Seeding the Blog table//
                ////////////////////////////////////////////////////////////////////////////////////

                //create blogs
                var blog1 = new Blog()
                {
                    Title = "Team expected to lose",
                    Text = "It seems that the football administration deams it unlikley hampten will win",
                    DatePosted = new DateTime(2021, 11, 23, 11, 2, 22),
                    Category = cat2,
                    Staff = jeff,
                    IsAnnouncement = true,

                };

                //create blogs
                var blog2 = new Blog()
                {
                    Title = "Is ibprofin bad?",
                    Text = "Scenitists all over the world have been debating weather or not ibprofin is bad or not. I don't know",
                    DatePosted = new DateTime(2021, 10, 11, 9, 7, 2),
                    Category = cat3,
                    Staff = alex
                };

                context.Blogs.Add(blog1);
                context.Blogs.Add(blog2);


                var comment1 = new Comment()
                {
                    Text = "Wow really interesting",
                    DateCommented = new DateTime(2021, 12, 12, 10, 10, 2),
                    Blog = blog2,
                    User = jeff,
                    IsConfimred = true

                };

                var comment2 = new Comment()
                {
                    Text = "Wow really interesting",
                    DateCommented = new DateTime(2021, 12, 22, 11, 4, 3),
                    Blog = blog1,
                    User = alex,
                    IsConfimred = true
                };

                context.Comments.Add(comment1);
                context.Comments.Add(comment2);


                //save changes to database
                context.SaveChanges();


                //Role Requests
                var request = new RoleRequest()
                {
                    DesiredRole = rolesAvailable.Admin,
                    Reason = "I want the power",
                    RequestTime = DateTime.Now,
                    User = alex
                };

                context.RoleRequests.Add(request);
                context.SaveChanges();

                //Styles
                var style = new Stylize()
                {
                    BackGroundColour = "#00ff90",
                    Font = "Alegreya",
                    User = jeff,
                    IsApproved = true,
                    RequestTime = new DateTime(2021, 11, 6, 9, 7, 2)
                };

                //Style Requests
                var style2 = new Stylize()
                {
                    BackGroundColour = "#00f0f0",
                    Font = "Alegreya",
                    User = alex,
                    IsApproved = false,
                    RequestTime = DateTime.Now
                };
                context.Stylizes.Add(style);
                context.Stylizes.Add(style2);
                context.SaveChanges();


            }

        }
    }
}