using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManytoManySnippet.Models
{
    public class MockInitializer : DropCreateDatabaseIfModelChanges<CourseContext>
    {
        protected override void Seed(CourseContext context)
        {
            base.Seed(context);

            var course1 = new Course { CourseID = 1,  CourseDescription = "Bird Watching" };
            var course2 = new Course { CourseID = 2, CourseDescription = "Basket weaving for beginners" };
            var course3 = new Course { CourseID = 3,  CourseDescription = "Photography 101" };

            context.Courses.Add(course1);
            context.Courses.Add(course2);
            context.Courses.Add(course3);
        }
    }
}