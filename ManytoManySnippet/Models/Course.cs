using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManytoManySnippet.Models
{
    public class Course
    {

        public int CourseID { get; set; }
        public string CourseDescription { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get;set ; }
    }
}