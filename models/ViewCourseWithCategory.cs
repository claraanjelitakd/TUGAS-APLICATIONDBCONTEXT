using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleRESTApi.Models;

namespace SimpleRESTApi.Models
{
    public class ViewCourseWithCategory
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = null!;
        public string? CourseDescription { get; set; } = null!; //tanda tanya setelah string artinya boleh null
        public double Duration { get; set; }
        public int categoryID { get; set; }
        public string categoryName { get; set; } = null!;
        
    }
}