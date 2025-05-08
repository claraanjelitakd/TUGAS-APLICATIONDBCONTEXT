using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using SimpleRESTApi.Models;

namespace SimpleRESTApi.Data
{
    public class CourseADO : ICourse
    {
         private IConfiguration _configuration; //dibuat readonlu
        private string connStr = string.Empty;

        public CourseADO(IConfiguration configuration) //configurasi dari appsettings.json
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }

        public Course AddCourse(Course Course)
        {
            using(SqlConnection conn = new SqlConnection (connStr))
            {
                string strsql = @"INSERT INTO Course (CourseName, CourseDescription, Duration, categoryID) VALUES (@CourseName, @CourseDescription, @Duration, @categoryID); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(strsql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@CourseName", Course.CourseName);
                    cmd.Parameters.AddWithValue("@CourseDescription", Course.CourseDescription);
                    cmd.Parameters.AddWithValue("@Duration", Course.Duration);
                    cmd.Parameters.AddWithValue("@categoryID", Course.categoryID);
                    conn.Open();
                    int CourseID = Convert.ToInt32(cmd.ExecuteScalar());
                    Course.CourseID = CourseID;
                    return Course;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public void DeleteCourse(int CourseID)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"DELETE FROM Course WHERE CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(strsql, conn);
                try                {
                    cmd.Parameters.AddWithValue("@CourseID", CourseID);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if(result == 0)
                    {
                        throw new Exception("Course not found");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
            }
        }
        }

        public ViewCourseWithCategory GetCourseById(int CourseID)
        {
            //select from view
            string strsql = @"SELECT CourseID, CourseName, CourseDescription, Duration, categoryID, categoryName FROM ViewCourseWithCategory WHERE CourseID = @CourseID";
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(strsql, conn))
            {
                cmd.Parameters.AddWithValue("@CourseID", CourseID);
                ViewCourseWithCategory course = new();
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            course.CourseID = Convert.ToInt32(dr["CourseID"]);
                            course.CourseName = dr["CourseName"]?.ToString() ?? string.Empty;
                            course.CourseDescription = dr["CourseDescription"]?.ToString() ?? string.Empty;
                            course.Duration = Convert.ToInt32(dr["Duration"]);
                            course.categoryID = Convert.ToInt32(dr["categoryID"]);
                            course.categoryName = dr["categoryName"]?.ToString() ?? string.Empty;
                        }
                    }
                    return course;
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public IEnumerable<ViewCourseWithCategory> GetCourses()
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"SELECT 
                            dbo.Course.CourseName, 
                            dbo.Course.Duration, 
                            dbo.Course.CourseDescription, 
                            dbo.Course.CourseID, 
                            dbo.Categories.categoryID AS Expr1, 
                            dbo.Categories.categoryName AS Expr2
                          FROM dbo.Categories 
                          INNER JOIN dbo.Course ON dbo.Categories.categoryID = dbo.Course.categoryID"; //yang berbeda perintah selectnya
                SqlCommand cmd = new SqlCommand(strsql, conn);
                List<ViewCourseWithCategory> Courses = new List<ViewCourseWithCategory>();
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if(dr.HasRows)
                    {
                        while(dr.Read())
                        {
                            ViewCourseWithCategory Course = new();
                            Course.CourseID = Convert.ToInt32(dr["CourseID"]); // Perbaikan: Baca kolom CourseID
                            Course.CourseName = dr["CourseName"].ToString();
                            Course.CourseDescription = dr["CourseDescription"].ToString();
                            Course.Duration = Convert.ToInt32(dr["Duration"]);
                            Course.categoryID = Convert.ToInt32(dr["Expr1"]); // Perbaikan: Gunakan alias Expr1
                            Course.categoryName = dr["Expr2"].ToString(); // Perbaikan: Gunakan alias Expr2
                            Courses.Add(Course);
                        }
                    }
                    return Courses;
                }
                catch(SqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public Course UpdateCourse(Course Course)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"UPDATE Course SET CourseName = @CourseName, CourseDescription = @CourseDescription, Duration = @Duration, categoryID = @categoryID
                WHERE CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(strsql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@CourseName", Course.CourseName);
                    cmd.Parameters.AddWithValue("@CourseDescription", Course.CourseDescription);
                    cmd.Parameters.AddWithValue("@Duration", Course.Duration);
                    cmd.Parameters.AddWithValue("@categoryID", Course.categoryID);
                    cmd.Parameters.AddWithValue("@CourseID", Course.CourseID);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        throw new Exception("Course not found");
                    }
                    return Course;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
        }
    }
    }
}