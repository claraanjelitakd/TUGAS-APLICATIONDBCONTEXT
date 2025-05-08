using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleRESTApi.Models;

namespace SimpleRESTApi.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = null!;
        public string CourseDescription { get; set; } = null!;
        public double Duration { get; set; }
        public int categoryID { get; set; }
        
    }
}