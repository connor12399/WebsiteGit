using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Blog
    {
        [Key]
       
        public int BlogId { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DatePosted { get; set; }

     //   public bool Approval { get; set; }

        public bool IsAnnouncement { get; set; }//only admins can make true


        //public Blog()
        //{
        //    Comments = new List<Comment>();
        //}

        //public virtual ICollection<Comment> Comments { get; set; }

        //Navigation - one
        [ForeignKey("Staff")]
        public string Id { get; set; }
        public Staff Staff { get; set; }

        //Navigation - one
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //Navigation -Many
        public List<Comment> Comments { get; set; }

    }
}