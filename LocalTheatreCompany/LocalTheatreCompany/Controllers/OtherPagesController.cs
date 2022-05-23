using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalTheatreCompany.Controllers
{
    public class OtherPagesController : Controller
    {
        // GET: OtherPages
        public ActionResult LandingPage()
        {
            return View();
        }
    }
}