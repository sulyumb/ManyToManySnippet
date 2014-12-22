using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManytoManySnippet.Models;
using ManytoManySnippet.ViewModels;
using System.Collections.ObjectModel;

namespace ManytoManySnippet.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel()
        {
            Courses = new Collection<AssignedCourseData>();
        }

        public int UserProfileID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AssignedCourseData> Courses { get;set ; }
    }
}