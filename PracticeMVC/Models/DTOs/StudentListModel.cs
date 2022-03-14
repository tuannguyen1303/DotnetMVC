using PracticeMVC.Models.Enum;

namespace PracticeMVC.Models.DTOs;

public class StudentListModel
{
    public Guid Id { get; set; }
    public int SN { get; set; }
    public string NRIC { get; set; }
    public string Name { get; set; }
    public GenderEnum Gender { get; set; }
    public int? Age { get; set; }
    public int? Subjects { get; set; }
}