using System;
using LocalTheatreCompany.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Net;

namespace LocalTheatreCompany.Controllers
{
    //Writer and admins can accese the staff controller, however admins have their own unique controller
    [Authorize(Roles = "Writer,Admin,Super-Admin")]//only allows user with either of those roles to access controller
    
    public class StaffController : Controller
    {
        private LTCDbContext db = new LTCDbContext();

        // GET: Staff
        //When user clicks "blog management" index is triggered
        //Index will retrn a list of blogs that were created by the user
        
        public ActionResult Index()
        {
            //Gets all the blogs from the blog table
            //Populates the foreign keys with data
            var blogs = db.Blogs.Include(b => b.Category).Include(b => b.Staff);

            //If the user is a writer the list of blogs returned to view will be only their blogs
            if (User.IsInRole("Writer"))
            {
                //Gets the user id of the logged in user
                var staffId = User.Identity.GetUserId();

                //Sets the list to the list of values within itself that have a staff
                blogs = blogs.Where(b => b.Staff.Id == staffId);
            }

            //Sends a list version of the blogs to the index view
            return View(blogs.ToList());
        }

        // GET: Staff/Details/5
        public ActionResult Details(int? id)//allows variable to be nullable
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //finds and stored the blog that matches the id passed in
            Blog blog = db.Blogs.Find(id);

            //will return a not found message if the blog isn't found
            if (blog == null)
            {
                return HttpNotFound();
            }

            //will return the passed in blog to the details view if it was found
            return View(blog);
        }

        // GET: Staff/Create
        //Creates blog
        public ActionResult Create()
        {
            //Sends list of categories to the view
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", "Pick One");
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogId, Title, Text, CategoryId, IsAnnouncement")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.DatePosted = DateTime.Now;

                blog.Id = User.Identity.GetUserId();

                db.Blogs.Add(blog);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", blog.Category);

            return View(blog);
        }

        // GET: Staff/Edit/5
        //This method will display the edit form to browser
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //finds and stored the blog that matches the id passed in
            Blog blog = db.Blogs.Find(id);

            //will return a not found message if the blog isn't found
            if (blog == null)
            {
                return HttpNotFound();
            }

            //Gets all the present categories that the user will be able to select
            //send it on a viewbag over to the view
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", blog.Category);

            return View(blog);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogId, Title, Text, CategoryId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                //Updates the new date
                blog.DatePosted = DateTime.Now;

                var userId = User.Identity.GetUserId();
                //Gets the id of the staff logged in and asigns it as foreign key
                blog.Id = userId;

                //Updates the database
                db.Entry(blog).State = EntityState.Modified;

                //saves changes to database
                db.SaveChanges();

                //Redirects the user to the index action
                return RedirectToAction("Index");
            }
            //Otherwise the blog is null
            //Send the list back to the edit form
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", blog.CategoryId);

            //returns blog to edit
            return View(blog);
        }

        // GET: Staff/Delete/5
        //Deletes methods a blog
        public ActionResult Delete(int? id)
        {
            //If id is null return error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find the blog of the id
            Blog blog = db.Blogs.Find(id);

            //Find the category by searching through category table
            var category = db.Categories.Find(blog.CategoryId);

            //set the category to the new category found
            blog.Category = category;

            //if the blog is null report not found
            if(blog == null)
            {
                return HttpNotFound();
            }

            return View(blog);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]//allows the method to be name somthing different
        public ActionResult Delete(int id)
        {
            //Finds the blogs by id 
            Blog blog = db.Blogs.Find(id);

            
            //Gets the blog comments
            blog.Comments = db.Comments.Where(c => c.BlogId == id).ToList();

            //Recusively deletes the comments
            //and those approval requests
            //foreach (var item in blog.Comments)
            //{
            //    db.Comments.Remove(item);
            //}
            for(int i = blog.Comments.Count; i > 0; i--)
            {
                db.Comments.Remove(blog.Comments.First());
            }

            //Removes blogs from blogs table
            db.Blogs.Remove(blog);


            //save database changes
            db.SaveChanges();

            //Index redirects
            return RedirectToAction("Index");
        }

        public ActionResult RequestAStyle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestAStyle(Stylize style)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var passedInUser = db.Users.Find(userId);

                //Attaches user to style
                style.User.Id = userId;

                style.RequestTime = DateTime.Now;

                //Save to db
                db.Stylizes.Add(style);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
