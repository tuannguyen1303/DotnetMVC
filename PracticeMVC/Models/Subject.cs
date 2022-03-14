using PracticeMVC.Models.Enum;

namespace PracticeMVC.Models;

public class Subject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public SubjectEnum Value { get; set; }

    public Guid StudentId { get; set; }
    public Student Student { get; set; }
}