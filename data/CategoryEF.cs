using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleRESTApi.Data;
using SimpleRESTApi.Models;

namespace SimpleRESTApi.data
{
    public class CategoryEF : ICategory
    {
        private readonly ApplicationDbContext _context;
        public CategoryEF(ApplicationDbContext context)
        {
            _context = context;
        }
        public category addCategory(category category)
        {
           try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category: " + ex.Message);
            }
        }

        public void deleteCategory(int categoryID)
        {
            var category = _context.Categories.FirstOrDefault(c => c.categoryID == categoryID);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category: " + ex.Message);
            }
        }

        public IEnumerable<category> GetCategories()
        {
            var category = _context.Categories.OrderByDescending(c => c.categoryID).ToList();
            return category;
        }

        public category GetCategoryById(int categoryID)
        {
            var category = _context.Categories.FirstOrDefault(c => c.categoryID == categoryID);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return category;
        }

        public category updateCategory(category category)
        {
           var existingCategory = _context.Categories.FirstOrDefault(c => c.categoryID == category.categoryID);
           if (existingCategory == null)
           {
               throw new Exception("Category not found");
           }
           try
           {
               existingCategory.categoryName = category.categoryName;
               _context.Categories.Update(existingCategory);
                _context.SaveChanges();
                return existingCategory;
            }      
            catch (Exception ex)
            {
                throw new Exception("Error updating category: " + ex.Message);
            }
        }
    }
}