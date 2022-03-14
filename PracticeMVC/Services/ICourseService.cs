using PracticeMVC.Models.DTOs;

namespace PracticeMVC.Services;

public interface ICourseService
{
    Task<bool> CreateCourse(StudentModel model);
    Task<List<StudentListModel>> GetList(string searchParams);
    Task<StudentModel> GetStudentById(Guid studentId);
    Task<bool> UpdateCourse(StudentModel model);
}