using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManytoManySnippet.Models;
using ManytoManySnippet.ViewModels;


namespace ManytoManySnippet.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly CourseContext db = new CourseContext();

        // GET: UserProfile
        public ActionResult Index()
        {
            var userProfiles =  db.UserProfiles.ToList();
            var userProfileViewModels = userProfiles.Select(userProfile => userProfile.ToViewModel()).ToList();

            return View(userProfileViewModels);
        }

        private ICollection<AssignedCourseData> PopulateCourseData()
        {
            var courses = db.Courses;
            var assignedCourses = new List<AssignedCourseData>();

            foreach (var item in courses)
            {
                assignedCourses.Add(new AssignedCourseData
                {
                    CourseID = item.CourseID,
                    CourseDescription = item.CourseDescription,
                    Assigned = false
                });
            }

            return assignedCourses;
        }

        public ActionResult Create()
        {
            var userProfileViewModel = new UserProfileViewModel { Courses = PopulateCourseData() };

            return View(userProfileViewModel);
        }

       

        [HttpPost]
        public ActionResult Create(UserProfileViewModel userProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                var userProfile = new UserProfile { Name = userProfileViewModel.Name };

                AddOrUpdateCourses(userProfile, userProfileViewModel.Courses);
                db.UserProfiles.Add(userProfile);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(userProfileViewModel);
        }

        //private void AddOrUpdateCourses(UserProfile userProfile, IEnumerable<AssignedCourseData> assignedCourses)
        //{
        //    if (assignedCourses != null)
        //    {
        //        foreach (var assignedCourse in assignedCourses)
        //        {
        //            if (assignedCourse.Assigned)
        //            {
        //                var course = new Course { CourseID = assignedCourse.CourseID };
        //                db.Courses.Attach(course);
        //                userProfile.Courses.Add(course);
        //            }
        //        }
        //    }
        //}

        private void AddOrUpdateCourses(UserProfile userProfile, IEnumerable<AssignedCourseData> assignedCourses)
        {
            if (assignedCourses == null) return;

            if (userProfile.UserProfileID != 0)
            {
                // Existing user - drop existing courses and add the new ones if any
                foreach (var course in userProfile.Courses.ToList())
                {
                    userProfile.Courses.Remove(course);
                }

                foreach (var course in assignedCourses.Where(c => c.Assigned))
                {
                    userProfile.Courses.Add(db.Courses.Find(course.CourseID));
                }
            }
            else
            {
                // New user
                foreach (var assignedCourse in assignedCourses.Where(c => c.Assigned))
                {
                    var course = new Course { CourseID = assignedCourse.CourseID };
                    db.Courses.Attach(course);
                    userProfile.Courses.Add(course);
                }
            }
        }

        public ActionResult Edit(int? id)
        {
            // Get all courses
            var allDbCourses = db.Courses.ToList();

            // Get the user we are editing and include the courses already subscribed to
            var userProfile = db.UserProfiles.Include("Courses").FirstOrDefault(x => x.UserProfileID == id);
            var userProfileViewModel =  userProfile.ToViewModel(allDbCourses); 

            return View(userProfileViewModel);
        }

        [HttpPost]
        public ActionResult Edit(UserProfileViewModel userProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                var originalUserProfile = db.UserProfiles.Find(userProfileViewModel.UserProfileID);
                AddOrUpdateCourses(originalUserProfile, userProfileViewModel.Courses);
                db.Entry(originalUserProfile).CurrentValues.SetValues(userProfileViewModel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(userProfileViewModel);
        }

        public ActionResult Details(int? id)
        {
            // Get all courses
            var allDbCourses = db.Courses.ToList();

            // Get the user we are editing and include the courses already subscribed to
            var userProfile = db.UserProfiles.Include("Courses").FirstOrDefault(x => x.UserProfileID == id);
            var userProfileViewModel = userProfile.ToViewModel(allDbCourses);

            return View(userProfileViewModel);
        }

        public ActionResult Delete(int id = 0)
        {
            var userProfileIQueryable = from u in db.UserProfiles.Include("Courses")
                                        where u.UserProfileID == id
                                        select u;

            if (!userProfileIQueryable.Any())
            {
                return HttpNotFound("User not found.");
            }

            var userProfile = userProfileIQueryable.First();
            var userProfileViewModel = userProfile.ToViewModel();

            return View(userProfileViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var userProfile = db.UserProfiles.Include("Courses").Single(u => u.UserProfileID == id);
            DeleteUserProfile(userProfile);

            return RedirectToAction("Index");
        }

        private void DeleteUserProfile(UserProfile userProfile)
        {
            if (userProfile.Courses != null)
            {
                foreach (var course in userProfile.Courses.ToList())
                {
                    userProfile.Courses.Remove(course);
                }
            }

            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
        }
    }
}