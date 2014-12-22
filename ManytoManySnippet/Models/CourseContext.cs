using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManytoManySnippet.Models
{
    public class CourseContext : DbContext
    {

        public CourseContext() : base("CourseContext")
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
            .HasMany(up => up.Courses)
            .WithMany(course => course.UserProfiles)
            .Map(mc =>
            {
                mc.ToTable("T_UserProfile_Course");
                mc.MapLeftKey("UserProfileID");
                mc.MapRightKey("CourseID");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}