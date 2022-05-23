using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using LocalTheatreCompany.Models;
using Microsoft.AspNet.Identity;
using System.Net;

namespace LocalTheatreCompany.Controllers
{
    public class HomeController : Controller
    {
        private LTCDbContext db = new LTCDbContext();
        public ActionResult Index()
        {
            //Gets all the announcements from the blog table
            //Populates the foreign keys with data
            var blogs = db.Blogs.Include(b => b.Category).Include(b => b.Staff).Where(b => b.IsAnnouncement);

            //Sends a list version of the blogs to the index view
            return View(blogs.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult BrowseBlogs()
        {
            //gets all the blogs, includes the categories and staff/writer for each blog
            //and orders the posts from newest to oldest
            var blogs = db.Blogs.Include(b => b.Category).Include(b => b.Staff).OrderByDescending(b => b.DatePosted);

            //sends the categories to the index page
            ViewBag.Categories = db.Categories.ToList();

            //sends all the blogs to the view
            return View(blogs.ToList());
        }

        [HttpPost]
        public ActionResult BrowseBlogs(string category)
        {
            //gets all the blogs, includes the categories and staff/writer for each blog
            //and orders the posts from newest to oldest
            var blogs = db.Blogs.Include(b => b.Category).Include(b => b.Staff).Where(b => b.Category.CategoryName == category).OrderByDescending(b => b.DatePosted);

            //sends the categories to the index page
            ViewBag.Categories = db.Categories.ToList();


            //sends all the blogs to the view
            return View(blogs.ToList());
        }

        //blog dettails
        //also contains blog comments
        public ActionResult Details(int id)
        {
            //searches the blogs context table using the id passed in
            //and returns a blog
            Blog blog = db.Blogs.Find(id);

            //searches the users context table using the id of the 
            //writer who created the blog, and returns that users details
            var user = db.Users.Find(blog.Id);

            //find category
            var category = db.Categories.Find(blog.CategoryId);

            //Gets all the comments that are linked to this blog
            var comments = db.Comments.Where(c => c.BlogId == id).Include(c => c.User).ToList();

            //Need to get the style from the user asell
            //Returns the first as their should only be one
            var style = db.Stylizes.Where(s => s.Id == user.Id).First();

            //addes user to the new blog details
            blog.Staff = (Staff) user;

            //adds categoriy to new blog details
            blog.Category = category;

            //adds comment tot the blog class
            blog.Comments = comments;

            ViewBag.Blog = blog;

            //Checks if user even has a style that is approved
            if (style.IsApproved)
            {
                //These viewbags will be used to personalize the look of the blogs
                //Trim to removes quotes
                ViewBag.Colour = style.BackGroundColour.Trim();
                ViewBag.Font = style.Font.Trim();
            }


            //Sends current userId to view
            ViewBag.UserId = User.Identity.GetUserId();

            var newComment = new Comment();

            return View(newComment);
        }


        [HttpPost]
        [ActionName("Details")]
        public ActionResult CommentCreation(Comment NewComment, int BlogId)
        {
            if (ModelState.IsValid)
            {
                NewComment.BlogId = BlogId;

                //Sets user id to comment,
                NewComment.Id = User.Identity.GetUserId();

                //Sets new date
                NewComment.DateCommented = DateTime.Now;

                //Adds to comment table
                db.Comments.Add(NewComment);

                db.SaveChanges();

                return RedirectToAction("Details");

            }

            return View();
        }

        [HttpGet]
        public ActionResult EditComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //finds and stored the blog that matches the id passed in
            Comment comment = db.Comments.Find(id);





            //will return a not found message if the comment isn't found
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = comment.BlogId;

            return View(comment);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(Comment comment, int blogId)
        {
            if (ModelState.IsValid)
            {

                //Updates the new date
                comment.DateCommented = DateTime.Now;

                //The comment needs re confirmed
                comment.IsConfimred = false;

                //Sets to Id
                comment.Id = User.Identity.GetUserId();
                comment.Blog = db.Blogs.Find(blogId);

                //Updates the database
                db.Entry(comment).State = EntityState.Modified;

                //saves changes to database
                db.SaveChanges();

                //Redirects the user to the index action
                return RedirectToAction("Index");
            }

            //returns blog to edit
            return RedirectToAction("BrowseBlogs");
        }



        //Will display the users details and let them edit some if they are logged in
        public ActionResult ViewProfile(int? id)
        {
            var loggedInUser = User.Identity.GetUserId();

            //Gets the user passed in from the list
            var passedInUser = db.Users.Find(id);

            //Same user so they can edit their profile
            if (id == null || loggedInUser == passedInUser.Id)
            {
                ViewBag.CanEdit = true;

                return View(db.Users.Find(loggedInUser));
            }
            //else

            ViewBag.CanEdit = false;

            return View(passedInUser);
        }

        public ActionResult EditProfile()
        {
            var userid = User.Identity.GetUserId();

            var user = db.Users.Find(userid);


            //will return a not found message if the user isn't found
            if (user == null)
            {
                return HttpNotFound();
            }


            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(User user)
        {
            if (ModelState.IsValid)
            {
                User currentUser = db.Users.Find(User.Identity.GetUserId());

                currentUser.FirstName = user.FirstName;

                currentUser.LastName = user.LastName;

                currentUser.Postcode = user.Postcode;

                //Updates the database
                db.Entry(currentUser).State = EntityState.Modified;

                //saves changes to database
                db.SaveChanges();

                //Redirects the user to the index action
                return RedirectToAction("ViewProfile");
            }

            //returns blog to edit
            return View(user);
        }


        public ActionResult RequestAROle()
        {
            ViewBag.AllRoles =  new rolesAvailable();
            return View();
        }

        [HttpPost]
        public ActionResult RequestAROle(RoleRequest roleRequest)
        {
            if (ModelState.IsValid)
            {
                //Sets the time to now
                roleRequest.RequestTime = DateTime.Now;

                //Attaches user to roleRequest
                roleRequest.Id = User.Identity.GetUserId();

                //Save to db
                db.RoleRequests.Add(roleRequest);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

       
        public ActionResult DeleteComment(int? id)
        {
            //Finds the comments by id 
            Comment comment = db.Comments.Find(id);


            //Removes comments from comments table
            db.Comments.Remove(comment);


            //save database changes
            db.SaveChanges();

            //Index redirects
            return RedirectToAction("Index");
        }

    }
}