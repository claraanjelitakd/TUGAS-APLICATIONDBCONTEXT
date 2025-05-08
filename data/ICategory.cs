using System;
using SimpleRESTApi.Models;
namespace SimpleRESTApi.Data
{
    public interface ICategory
    {
        //crud
        IEnumerable<category> GetCategories();
        category GetCategoryById(int categoryID);
        category addCategory(category category);
        category updateCategory(category category);
        void deleteCategory(int categoryID);
    }
}