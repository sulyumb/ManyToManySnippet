using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManytoManySnippet.Models
{
    public class AssignedCourseData
    {
        public int CourseID { get; set; }
        public string CourseDescription { get; set; }
        public bool Assigned { get; set; }
    }
}