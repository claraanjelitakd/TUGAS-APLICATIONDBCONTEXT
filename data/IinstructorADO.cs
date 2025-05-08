using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SimpleRESTApi.Models;

namespace SimpleRESTApi.Data
{
    public class IinstructorADO : IInstructor
    {
        private IConfiguration _configuration;
        private string connStr = string.Empty;
        public IinstructorADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }
        public Instructor addInstructor(Instructor Instructor)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"INSERT INTO Instructor (InstructorName,InstructorEmail,InstructorPhone,InstructorAddress,InstructorCity) VALUES (@InstructorName,@InstructorEmail,@InstructorPhone,@InstructorAddress,@InstructorCity); SELECT SCOPE_IDENTITY()"; //mengambil data dari tabel --> membuat urut dari ID bukan dari name
                SqlCommand cmd = new SqlCommand(strsql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@InstructorName", Instructor.InstructorName);
                    cmd.Parameters.AddWithValue("@InstructorEmail", Instructor.InstructorEmail);
                    cmd.Parameters.AddWithValue("@InstructorPhone", Instructor.InstructorPhone);
                    cmd.Parameters.AddWithValue("@InstructorAddress", Instructor.InstructorAddress);
                    cmd.Parameters.AddWithValue("@InstructorCity", Instructor.InstructorCity);
                    conn.Open();
                    int InstructorID = Convert.ToInt32(cmd.ExecuteScalar());
                    Instructor.InstructorID = InstructorID;
                    return Instructor;
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

        public void deleteInstructor(int InstructorID)
        {
           using(SqlConnection conn = new SqlConnection(connStr))
           {
               string strsql = @"DELETE FROM Instructor WHERE InstructorID = @InstructorID"; //mengambil data dari tabel --> membuat urut dari ID bukan dari name
               SqlCommand cmd = new SqlCommand(strsql, conn);
               try
               {
                   cmd.Parameters.AddWithValue("@InstructorID", InstructorID);
                   conn.Open();
                   int result = cmd.ExecuteNonQuery();
                    if (result==0)
                    {
                        throw new Exception("Category not found");
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

        public Instructor GetInstructorById(int InstructorID)
        {
            Instructor Instructor = new();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"SELECT * FROM Instructor WHERE InstructorID = @Instructor";
                SqlCommand cmd = new SqlCommand(strsql, conn);
                cmd.Parameters.AddWithValue("@Instructor", InstructorID);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader(); //baca data pakai data reader trs dimaping make while
                if (dr.HasRows)
                {
                   while(dr.Read())
                    {
                    Instructor.InstructorID = Convert.ToInt32(dr["InstructorID"]);
                        Instructor.InstructorName = dr["InstructorName"].ToString();
                        Instructor.InstructorEmail = dr["InstructorEmail"].ToString();
                        Instructor.InstructorPhone = dr["InstructorPhone"].ToString();
                        Instructor.InstructorAddress = dr["InstructorAddress"].ToString();
                        Instructor.InstructorCity = dr["InstructorCity"].ToString();
                    }
                    
                }
                else{throw new Exception("Instructor not found");}
            }
            return Instructor;

        }

        public IEnumerable<Instructor> GetInstructors()
        {
            List<Instructor> Instructors = new List<Instructor>(); //diluar using
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"SELECT * FROM Instructor";
                SqlCommand cmd = new SqlCommand(strsql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader(); //baca data pakai data reader trs dimaping make while
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Instructor Instructor = new();
                        Instructor.InstructorID = Convert.ToInt32(dr["InstructorID"]);
                        Instructor.InstructorName = dr["InstructorName"].ToString();
                        Instructor.InstructorEmail = dr["InstructorEmail"].ToString();
                        Instructor.InstructorPhone = dr["InstructorPhone"].ToString();
                        Instructor.InstructorAddress = dr["InstructorAddress"].ToString();
                        Instructor.InstructorCity = dr["InstructorCity"].ToString();
                        Instructors.Add(Instructor);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return Instructors;
        }

        public Instructor updateInstructor(Instructor Instructor)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"UPDATE Instructor SET InstructorName = @InstructorName WHERE InstructorID = @InstructorID"; //mengambil data dari tabel --> membuat urut dari ID bukan dari name
                SqlCommand cmd = new SqlCommand(strsql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@InstructorName", Instructor.InstructorName);
                    cmd.Parameters.AddWithValue("@InstructorID", Instructor.InstructorID);
                    cmd.Parameters.AddWithValue("@InstructorEmail", Instructor.InstructorEmail);
                    cmd.Parameters.AddWithValue("@InstructorPhone", Instructor.InstructorPhone);
                    cmd.Parameters.AddWithValue("@InstructorAddress", Instructor.InstructorAddress);
                    cmd.Parameters.AddWithValue("@InstructorCity", Instructor.InstructorCity);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result==0)
                    {
                        throw new Exception("Instructor not found");
                    }
                    return Instructor;
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