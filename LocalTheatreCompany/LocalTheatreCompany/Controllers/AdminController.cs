using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LocalTheatreCompany.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using LocalTheatreCompany.Models;

namespace LocalTheatreCompany.Controllers
{
    [Authorize(Roles =  "Admin,Super-Admin")]//allows only admin to utilize this controller
    public class AdminController : Controller
    {
        //initialises database for usage
        private LTCDbContext db = new LTCDbContext();

        //Display choices
        public ActionResult AdminMenu()
        {
            return View();
        } 
        
        
        // GET: Users
        public ActionResult UserManagement()
        {
            //Gets all the users in the database including their role
            //and orders by first name
            //and converts to list
            List<User> users = db.Users.OrderBy(u => u.FirstName).ToList();
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(db));
                                                    //UserId  ROle

            ViewBag.UserCurrentRoles = new Dictionary<string, string>();

            foreach (var item in users)
            {
                
                ViewBag.UserCurrentRoles.Add(item.Id, userManager.GetRoles(item.Id)[0]);

            }

            return View(users);
        }


        // GET: Admin/Details/5
        public ActionResult CategoryList()
        {
            //returns view of category list and passeas in the database values of categories
            return View(db.Categories.ToList());
        }

        // GET: Admin/Create
        public ActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult CreateCategory([Bind(Include = "CategoryId, Name")]Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("CategoryList");
            }

            //Returns category to the view without it actually being added to the view
            return View(category);
        }

        public ActionResult DeleteCategory(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);

            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult DeleteCategoryConfirm(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("CategoryList");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AdminInbox()
        {

            //List of comments with users that made the comment
            List<Comment> comments = db.Comments.Include(c => c.User)
                //Adds only comments that aren't confirmed
                .Where(c => c.IsConfimred == false)
                //Orders by date so oldest are seen first
                .OrderBy(u => u.DateCommented).ToList();

                return View(comments);
        }

        public ActionResult Approve(int commentId)
        {
            Comment comment = db.Comments.Find(commentId);

            //Sets confirmed to true
            comment.IsConfimred = true;

            //Updates database
            db.Entry(comment).State = EntityState.Modified;

            //Save changes
            db.SaveChanges();

            //Redirects the user to the index action
            return RedirectToAction("AdminInbox");

        }

        public ActionResult Delete(int? commentId)
        {
            //If id is null return error
            if (commentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Find(commentId);

            //Deletes comment
            db.Comments.Remove(comment);

            //Save changes
            db.SaveChanges();

            //Redirects the user to the index action
            return RedirectToAction("AdminInbox");
        }

        public ActionResult SuperAdminInbox()
        {
            List<RoleRequest> roleRequests = db.RoleRequests.Include(c => c.User)
                //Orders by date so oldest are seen first
                .OrderBy(u => u.RequestTime)
                .ToList();

            return View(roleRequests);
        }

        [Authorize(Roles = "Super-Admin")]
        public ActionResult Promote(int roleRequestId)
        {
            //Gets the role request
            RoleRequest roleRequest = db.RoleRequests.Find(roleRequestId);

            //Initilises role manager required for use
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            //Initilises role manager required for use
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(db));

            //Gets the user id and sets it to an easily understandable variable
            string userId = roleRequest.Id;

            //Gets the role and sets it to an easily understandable variable
            string role = roleRequest.DesiredRole.ToString();

            //Finds the old user, this is very important
            var olduser = userManager.FindById(userId);


            //Gets old role
            var oldrole = roleManager.FindById(olduser.Roles.FirstOrDefault().RoleId);

            //If they are a member then their class will just be a user
            //So we need to upgrade their class to staff
            if(oldrole.ToString() == "Member")
            {
                Staff newStaff = (Staff) olduser;

                newStaff.DatePromoted = DateTime.Now;
            }

            //Removes role
            userManager.RemoveFromRole(userId, oldrole.Name);

            //Adds new role
            var result = userManager.AddToRole(userId, role);

            //Redirects the user to the index action
            return RedirectToAction("SuperAdminInbox");
        }

        public ActionResult SuspendAUser(string Id)
        {
            //Gets the role request
            User userSuspend = db.Users.Find(Id);

            //Initilises role manager required for use
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            //Initilises role manager required for use
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(db));

            var oldrole = roleManager.FindById(userSuspend.Roles.FirstOrDefault().RoleId);

            //Prevents Super-Admin from suspending another
            if(oldrole.Name == "Super-Admin")
            {
                return RedirectToAction("UserManagement");
            }

            //Removes role
            userManager.RemoveFromRole(userSuspend.Id, oldrole.Name);

            //Adds new role
            var result = userManager.AddToRole(userSuspend.Id, "Suspended");

            //Redirects the user to the index action
            return RedirectToAction("UserManagement");
        }

        [Authorize(Roles = "Super-Admin")]
        public ActionResult DeleteRoleRequest(int? roleRequestId)
        {
            //If id is null return error
            if (roleRequestId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RoleRequest roleRequest = db.RoleRequests.Find(roleRequestId);

            //Deletes comment
            db.RoleRequests.Remove(roleRequest);

            //Save changes
            db.SaveChanges();

            //Redirects the user to the index action
            return RedirectToAction("SuperAdminInbox");
        }

        [Authorize(Roles = "Super-Admin")]
        public ActionResult SuperAdminInboxStyle()
        {
            List<Stylize> styles = db.Stylizes.Include(s => s.User)
                //Only display the once that are'nt appproved
                .Where(s => s.IsApproved == false)
                //Orders by date so oldest are seen first
                .OrderBy(s => s.RequestTime)
                .ToList();

            return View(styles);
        }
        public ActionResult AllowStyle(int styleId)
        {
            //Gets the style
            Stylize style = db.Stylizes.Find(styleId);

            //If statments in views will now change the colours and fonts
            style.IsApproved = true;

            //Updates the database
            db.Entry(style).State = EntityState.Modified;

            //saves changes to database
            db.SaveChanges();

            return RedirectToAction("SuperAdminInboxStyle");
        }

        public ActionResult DeleteStyleRequest(int? styleId)
        {
            //If id is null return error
            if (styleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stylize style = db.Stylizes.Find(styleId);

            //Deletes comment
            db.Stylizes.Remove(style);

            //Save changes
            db.SaveChanges();

            //Redirects the user to the index action
            return RedirectToAction("SuperAdminInbox");
        }
    }
}
