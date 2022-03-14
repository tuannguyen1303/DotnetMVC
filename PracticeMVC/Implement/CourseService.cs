using Microsoft.EntityFrameworkCore;
using PracticeMVC.DBContext;
using PracticeMVC.Models;
using PracticeMVC.Models.DTOs;
using PracticeMVC.Models.Enum;
using PracticeMVC.Services;

namespace PracticeMVC.Implement;

public class CourseService : ICourseService
{
    private readonly CourseContext _context;
    private readonly ILogger<CourseService> _logger;

    public CourseService(CourseContext context, ILogger<CourseService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateCourse(StudentModel model)
    {
        _logger.LogInformation(nameof(CreateCourse));
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var student = new Student
            {
                Id = Guid.NewGuid(),
                Birthday = model.BirthDay,
                Gender = model.GenderEnum,
                Name = model.Name,
                NRIC = model.NRIC,
                AvailableDate = model.AvailableDate
            };

            var listSubjects = new List<Subject>();

            if (model.Subjects != null && model.Subjects.Any())
            {
                listSubjects = model.Subjects.Select(m => new Subject
                {
                    Id = Guid.NewGuid(),
                    Name = Enum.GetName(m),
                    Value = m,
                    StudentId = student.Id
                }).ToList();
            }

            _context.Students.Add(student);
            if (listSubjects.Any()) _context.Subjects.AddRange(listSubjects);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError(e, $"{nameof(CreateCourse)}: {e.Message}");
            throw new Exception(e.Message);
        }
    }

    public async Task<List<StudentListModel>> GetList(string searchParams)
    {
        _logger.LogInformation(nameof(GetList));
        try
        {
            var currentYear = DateTime.Now.Year;
            var query = (from students in _context.Students
                    join subjects in _context.Subjects on students.Id equals subjects.StudentId into sub
                    from s in sub.DefaultIfEmpty()
                    where (string.IsNullOrEmpty(searchParams) || students.Name.Contains(searchParams))
                          || (string.IsNullOrEmpty(searchParams) || students.NRIC.Contains(searchParams))
                    orderby students.Id
                    select new StudentListModel
                    {
                        Id = students.Id,
                        Gender = students.Gender,
                        Name = students.Name,
                        NRIC = students.NRIC,
                        Subjects = students.Subjects != null
                            ? students.Subjects.Count(item => item.Id != Guid.Empty)
                            : 0,
                        Age = currentYear - students.Birthday.Year
                    }).GroupBy(item => item.Id)
                .Select(item => item.OrderBy(p => p.Name).First());

            var result = await query.ToListAsync();
            if (result.Any())
            {
                var index = 1;
                result.ForEach(item =>
                {
                    item.SN = index;
                    index++;
                });
            }

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{nameof(GetList)}: {e.Message}");
            throw new Exception(e.Message);
        }
    }

    public async Task<StudentModel> GetStudentById(Guid studentId)
    {
        _logger.LogInformation(nameof(GetStudentById));
        try
        {
            var query = await (from students in _context.Students
                join subjects in _context.Subjects on students.Id equals subjects.StudentId into sb
                from s in sb.DefaultIfEmpty()
                where students.Id == studentId
                select new StudentModel
                {
                    Id = students.Id,
                    Name = students.Name,
                    AvailableDate = students.AvailableDate,
                    BirthDay = students.Birthday,
                    GenderEnum = students.Gender,
                    NRIC = students.NRIC,
                    Subjects = students.Subjects != null && students.Subjects.Any()
                        ? students.Subjects.Select(sub => sub.Value).ToList()
                        : null,
                    SubjectValues = students.Subjects != null && students.Subjects.Any()
                        ? students.Subjects.Select(sub => (int) sub.Value).ToList()
                        : null
                }).FirstOrDefaultAsync();

            return query ?? new StudentModel();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{nameof(GetStudentById)}: {e.Message}");
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> UpdateCourse(StudentModel model)
    {
        _logger.LogInformation(nameof(UpdateCourse));
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var currentStudent = await _context.Students.FirstOrDefaultAsync(p => p.Id == model.Id!.Value);
            var subjects = await _context.Subjects.Where(p => p.StudentId == model.Id!.Value).ToListAsync();

            if (currentStudent != null)
            {
                currentStudent.Birthday = model.BirthDay;
                currentStudent.Gender = model.GenderEnum;
                currentStudent.Name = model.Name;
                currentStudent.NRIC = model.NRIC;
                currentStudent.AvailableDate = model.AvailableDate;

                _context.Students.Update(currentStudent);
                
                if (subjects.Any())
                    _context.Subjects.RemoveRange(subjects);
                
                if (model.SubjectValues != null && model.SubjectValues.Any())
                {
                    var listSubjects = model.SubjectValues.Select(m =>
                    {
                        var enumValue = (SubjectEnum)Enum.ToObject(typeof(SubjectEnum), m);
                        var sub = new Subject
                        {
                            Id = Guid.NewGuid(),
                            Name = Enum.GetName(enumValue),
                            Value = enumValue,
                            StudentId = currentStudent.Id
                        };

                        return sub;
                    }).ToList();

                    _context.Subjects.AddRange(listSubjects);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            
            _logger.LogError(e, $"{nameof(GetStudentById)}: {e.Message}");
            return false;
        }
    }
}