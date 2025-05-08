using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using SimpleRESTApi.Models;

namespace SimpleRESTApi.Data
{
    public interface ICourse
    {
        //CRUD
        IEnumerable<ViewCourseWithCategory> GetCourses();
        ViewCourseWithCategory GetCourseById(int CourseID); //mengambil data berdasarkan ID di view
        Course AddCourse(Course Course);
        Course UpdateCourse(Course Course);
        void DeleteCourse(int CourseID);

    }
}