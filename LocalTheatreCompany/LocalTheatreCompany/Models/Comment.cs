using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocalTheatreCompany.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Text { get; set; }

        [Display(Name = "Date Commented")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateCommented { get; set; }

        public bool IsConfimred { get; set; }

        //Navigation - one
        [ForeignKey("Blog")]
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        [ForeignKey("User")]
        public string Id { get; set; }
        public User User { get; set; }
    }
}