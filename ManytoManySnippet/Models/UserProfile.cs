﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManytoManySnippet.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            Courses = new List<Course>();
        }

        public int UserProfileID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}