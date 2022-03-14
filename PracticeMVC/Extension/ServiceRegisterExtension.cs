using PracticeMVC.Implement;
using PracticeMVC.Services;

namespace PracticeMVC.Extension;

public static class ServiceRegisterExtension
{
    public static void RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICourseService, CourseService>();
    }
}