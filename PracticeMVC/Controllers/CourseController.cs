using Microsoft.AspNetCore.Mvc;
using PracticeMVC.Models.DTOs;
using PracticeMVC.Models.Enum;
using PracticeMVC.Services;

namespace PracticeMVC.Controllers;

public class CourseController : Controller
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CourseController> _logger;

    public CourseController(ICourseService courseService, ILogger<CourseController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string searchParams)
    {
        try
        {
            ViewData["SearchParams"] = searchParams;
            var result = await _courseService.GetList(searchParams);
            if (result.Any()) return View(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  $"Failed to load data: {nameof(Index)}");
        }
        return View();
    }

    public IActionResult Create()
    {
        BindingValueForForm();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentModel model)
    {
        try
        {
            BindingValueForForm();
            if (ModelState.IsValid)
            {
                var listSubject = model.SubjectValues != null && model.SubjectValues.Any()
                    ? model.SubjectValues.Select(m => (SubjectEnum) m).ToList()
                    : new List<SubjectEnum>();
                model.Subjects = listSubject;
            
                await _courseService.CreateCourse(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        catch (Exception)
        {
            ViewData["MessageError"] = "Failed to create";
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        BindingValueForForm();
        try
        {
            var studentModel = await _courseService.GetStudentById(id);
            return View(studentModel);
        }
        catch (Exception)
        {
            ViewData["MessageError"] = "Failed to load record";
            return View(new StudentModel());
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(StudentModel model)
    {
        BindingValueForForm();
        try
        {
            if (ModelState.IsValid)
            {
                var result = await _courseService.UpdateCourse(model);
                if (result) return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        catch (Exception e)
        {
            ViewData["MessageError"] = "Failed to update";
            return View(model);
        }
    }

    private void BindingValueForForm()
    {
        ViewData["Subjects"] = Enum.GetValues(typeof(SubjectEnum))
            .Cast<SubjectEnum>()
            .ToDictionary(t => t.ToString(), t => (int)t);

        ViewData["Genders"] = Enum.GetValues(typeof(GenderEnum))
            .Cast<GenderEnum>()
            .ToDictionary(t => t.ToString(), t => (int) t);
    }
}