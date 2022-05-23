using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        //Navigation -Many
        public List<Blog> Blogs { get; set; }
    }
}