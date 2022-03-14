using Microsoft.EntityFrameworkCore;
using PracticeMVC.Models;
using PracticeMVC.Models.Enum;

namespace PracticeMVC.DBContext;

public class CourseContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    
    public CourseContext(DbContextOptions<CourseContext> options) : base(options)
    {
        
    }
}