using System;
using SimpleRESTApi.Models;
namespace SimpleRESTApi.Data;
public class categoryDal : ICategory
{
    private List<category> _categories = new List<category>();
    public categoryDal()
    {
        _categories = new List<category>
        {
            new category { categoryID = 1, categoryName = "ASP.NET Core" },
            new category { categoryID = 2, categoryName = "ASP.NET Core MVC" },
            new category { categoryID = 3, categoryName = "ASP.NET Core Web API" },
            new category { categoryID = 4, categoryName = "Blazor" },
            new category { categoryID = 5, categoryName = "Xamarin" },
            new category { categoryID = 6, categoryName = "Azure" }
        };
    }
        public IEnumerable<category> GetCategories()
    {
        return _categories;
    }
    public category GetCategoryById(int categoryID)
    {
        var category = _categories.FirstOrDefault(c => c.categoryID == categoryID);
        if (category == null)
        {
            throw new Exception ("Category not found");
        }
        return category;
    }
    public category addCategory(category category)
    {
        _categories.Add(category);
        return category;
    }

    public void deleteCategory(int categoryID)
    {
       var category = GetCategoryById(categoryID);
       if (category != null)
       {
           _categories.Remove(category);
       }
       else
       {
           throw new Exception("Category not found");
       }
    }

    public category updateCategory(category category)
    {
        var existingCategory = GetCategoryById(category.categoryID);
        if (existingCategory != null)
        {
            existingCategory.categoryName = category.categoryName;
        }
        else
        {
            throw new Exception("Category not found");
        }
        return existingCategory;
    }
}