using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManytoManySnippet.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get;set ;}
        public string CourseDescription { get; set; }
        public virtual ICollection<UserProfileViewModel> UserProfiles { get; set; }
    }
}