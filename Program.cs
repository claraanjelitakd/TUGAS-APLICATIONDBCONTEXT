using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleRESTApi.data;
using SimpleRESTApi.Data;
using SimpleRESTApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//DI --> di injek lalu nanti baru bisa dipakai dibawahnya.
builder.Services.AddScoped<ICategory, CategoryADO>(); // Ubah dari AddSingleton ke AddScoped //dari ado.net --> diubah biar bisa ngambil dari json krn klo singleton bs ada resiko deadlock krn request scr bersamaan klo scope -->itu transient untuk ngatur dari iconfiguration. 
//builder.Services.AddSingleton<IInstructor, InstructorDal>();
builder.Services.AddScoped<IInstructor, IinstructorADO>();
builder.Services.AddScoped<ICourse, CourseADO>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICategory, CategoryEF>();
builder.Services.AddScoped<IInstructor, InstructorEF>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
//app.MapGet("api/v1/helloservices/", (string? id) => $"Hello {id}!");
//app.MapGet("api/v1/helloservices/{name}", (string name) => $"Hello {name}!");
app.MapGet("api/v1/luas-segitiga", (double alas, double tinggi) =>
{
    var luas = alas * tinggi / 2;
    return $"Luas segitiga dengan alas {alas} dan tinggi {tinggi} adalah {luas}";
});
// app.MapGet("api/v1/categories",() =>
// {
//     category category = new category();
//     category.categoryID = 1;
//     category.categoryName = "ASP.NET core";
//     return category;
// });
app.MapGet("api/v1/categories",(ICategory categoryData) =>
{
    var categories = categoryData.GetCategories();
    return categories;
});

app.MapGet("api/v1/categories/{id}",(ICategory categoryData, int id) =>
{
    var category = categoryData.GetCategoryById(id);
    return category;
});

app.MapPost("api/v1/categories",(ICategory categoryData, category category) =>
{
    var newCategory = categoryData.addCategory(category);
    return newCategory;
});

app.MapPut("api/v1/categories",(ICategory categoryData, category category) =>
{
    var updatedCategory = categoryData.updateCategory(category);
    return updatedCategory;
});
app.MapDelete("api/v1/categories/{id}",(ICategory categoryData, int id) =>
{
    categoryData.deleteCategory(id);
    return Results.NoContent();
});
app.MapGet("api/v1/instrutors",() =>
{
    var instructor = new Instructor
    {
        InstructorID = 1,
        InstructorName = "Erick Kurniawan",
        InstructorEmail = "Erick@gmail.com",
        InstructorPhone = "08123456789",
        InstructorAddress = "Jl. Jendral Sudirman",
        InstructorCity = "Jakarta"
    };
    return instructor;
});
app.MapGet("api/v1/instructor",(IInstructor instructorData) =>
{
    var instructors = instructorData.GetInstructors();
    return instructors;
});

app.MapGet("api/v1/instructor/{id}",(IInstructor instructorData, int id) =>
{
    var instructor = instructorData.GetInstructorById(id);
    return instructor;
});
app.MapPost("api/v1/instructor",(IInstructor instructorData, Instructor instructor) =>
{
    var newInstructor = instructorData.addInstructor(instructor);
    return newInstructor;
});
app.MapDelete("api/v1/instructor/{id}",(IInstructor instructorData, int id) =>
{
    instructorData.deleteInstructor(id);
    return Results.NoContent();
});
app.MapPut("api/v1/instructor",(IInstructor instructorData, Instructor instructor) =>
{
    var updatedInstructor = instructorData.updateInstructor(instructor);
    return updatedInstructor;
});

app.MapGet("api/v1/courses",(ICourse courseData) =>
{
    var courses = courseData.GetCourses();
    return courses;
});
app.MapDelete("api/v1/courses/{id}",(ICourse courseData, int id) =>
{
    courseData.DeleteCourse(id);
    return Results.NoContent();
});
app.MapPut("api/v1/courses",(ICourse courseData, Course course) =>
{
    var updatedCourse = courseData.UpdateCourse(course);
    return updatedCourse;
});
app.MapPost("api/v1/courses",(ICourse courseData, Course course) =>
{
    var newCourse = courseData.AddCourse(course);
    return newCourse;
});
app.MapGet("api/v1/courses/{id}",(ICourse courseData, int id) =>
{
    var course = courseData.GetCourseById(id);
    return course;
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
