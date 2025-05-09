using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using SimpleRESTApi.data;
using SimpleRESTApi.Models;


namespace SimpleRESTApi.Data
{
    public class CourseEF: ICourse
    {
        private readonly ApplicationDbContext _context;
        public CourseEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course AddCourse(Course Course)
        {
            try
            {
                _context.Course.Add(Course);
                _context.SaveChanges();
                return Course;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding course: " + ex.Message);
            }
        }

        public void DeleteCourse(int CourseID)
        {
            var Course = _context.Course.FirstOrDefault(c => c.CourseID == CourseID);
            if (Course == null)
            {
                throw new Exception("Course not found");
            }
            try
            {
                _context.Course.Remove(Course);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting course: " + ex.Message);
            }
        }

        public ViewCourseWithCategory GetCourseById(int CourseID)
        {
            var course = (from c in _context.Course
                          join category in _context.Categories on c.categoryID equals category.categoryID
                          where c.CourseID == CourseID
                          select new ViewCourseWithCategory
                          {
                              CourseID = c.CourseID,
                              CourseName = c.CourseName,
                              CourseDescription = c.CourseDescription,
                              Duration = c.Duration,
                              categoryID = c.categoryID,
                              categoryName = category.categoryName
                          }).FirstOrDefault();
            if (course == null)
            {
                throw new Exception("Course not found");
            }
            return course;
        }

        public IEnumerable<ViewCourseWithCategory> GetCourses()
        {
            var Course = (from c in _context.Course
                           join cat in _context.Categories on c.categoryID equals cat.categoryID
                           select new ViewCourseWithCategory
                           {
                               CourseID = c.CourseID,
                               CourseName = c.CourseName,
                               CourseDescription = c.CourseDescription,
                               Duration = c.Duration,
                               categoryID = c.categoryID,
                               categoryName = cat.categoryName
                           }).ToList();
            return Course;
        }

        public Course UpdateCourse(Course Course)
        {
            var existingCourse = _context.Course.FirstOrDefault(c => c.CourseID == Course.CourseID);
            if (existingCourse == null)
            {
                throw new Exception("Course not found");
            }
            try
            {
                existingCourse.CourseName = Course.CourseName;
                existingCourse.CourseDescription = Course.CourseDescription;
                existingCourse.Duration = Course.Duration;
                existingCourse.categoryID = Course.categoryID;
                _context.SaveChanges();
                return existingCourse;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating course: " + ex.Message);
            }
        }
    }

}