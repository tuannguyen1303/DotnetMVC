using PracticeMVC.Models.Enum;

namespace PracticeMVC.Models;

public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string NRIC { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime? AvailableDate { get; set; }
    public GenderEnum Gender { get; set; }

    public List<Subject>? Subjects { get; set; }
}