using Microsoft.EntityFrameworkCore;
using PracticeMVC.DBContext;

namespace PracticeMVC.Extension;

public static class DbContextExtension
{
    public static void AddDbContextExtension(this IServiceCollection serviceCollection, 
        WebApplicationBuilder builder)
    {
        serviceCollection.AddDbContext<CourseContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("CourseContext"));
        });
    }
}