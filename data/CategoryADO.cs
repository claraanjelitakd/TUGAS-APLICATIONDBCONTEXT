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
    public class CategoryADO : ICategory
    {
        private IConfiguration _configuration;
        private string connStr = string.Empty;

        public CategoryADO(IConfiguration configuration) //configurasi dari appsettings.json
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }
        public category addCategory(category category)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
              string strsql = @"INSERT INTO categories (categoryName) VALUES (@categoryName); SELECT SCOPE_IDENTITY()"; //mengambil data dari tabel --> membuat urut dari ID bukan dari name
              SqlCommand cmd = new SqlCommand(strsql, conn);
              try
              {

                    cmd.Parameters.AddWithValue("@categoryName", category.categoryName);
                    conn.Open();
                    int categoryID = Convert.ToInt32(cmd.ExecuteScalar());
                    category.categoryID = categoryID;
                    return category;

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

        public void deleteCategory(int categoryID)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
            string strsql = @"DELETE FROM categories WHERE categoryID = @categoryID"; //mengambil data dari tabel --> membuat urut dari ID bukan dari name
            SqlCommand cmd = new SqlCommand(strsql, conn);
            try{
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result==0)
                {
                    throw new Exception("Category not found");
                }
            }
            catch(Exception ex)
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

        public IEnumerable<category> GetCategories()
        {
            List<category> categories = new List<category>(); //diluar using
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"SELECT*FROM categories ORDER BY categoryID"; //mengambil data dari tabel --> membuat urut dari ID bukan dari name
                SqlCommand cmd = new SqlCommand(strsql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader(); //baca data pakai data reader trs dimaping make while
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        //dimaping di class
                        category category = new();
                        category.categoryID = Convert.ToInt32(dr["CategoryID"]);
                        category.categoryName = dr["CategoryName"].ToString();
                        categories.Add(category); //di add karena datanya lebih dari 1
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close(); // sebenernya gausa di close gapapa soalnya di end of scope krn make using ini otomatis di close konseksinya.

            }
            return categories;

        }

        public category GetCategoryById(int categoryID)
        {
            category category = new();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"SELECT*FROM categories WHERE categoryID = @category"; //mengambil data dari tabel --> membuat urut dari ID bukan dari name
                //jangan pakai string biasa untuk menghindari sql injeksion di sanitize dulu
                SqlCommand cmd = new SqlCommand(strsql, conn);
                cmd.Parameters.AddWithValue("@category", categoryID);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader(); //baca data pakai data reader trs dimaping make while
                if(dr.HasRows)
                {
                    dr.Read();
                    //dimaping di class
                    
                    category.categoryID = Convert.ToInt32(dr["categoryID"]);
                    category.categoryName = dr["categoryName"].ToString();
                }      
                else
                {throw new Exception("Category not found");}
            }
            return category;
        }

        public category updateCategory(category category)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strsql = @"UPDATE categories SET categoryName = @categoryName
                WHERE categoryID = @categoryID"; //mengambil data dari tabel --> membuat urut dari ID bukan dari name
                SqlCommand cmd = new SqlCommand(strsql, conn);
                try{
                    cmd.Parameters.AddWithValue("@categoryName", category.categoryName);
                    cmd.Parameters.AddWithValue("@categoryID", category.categoryID);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result==0)
                    {
                        throw new Exception("Category not found");
                    }
                    return category;
                }
                catch(Exception ex)
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