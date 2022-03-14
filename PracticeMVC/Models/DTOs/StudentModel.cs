using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PracticeMVC.Models.Enum;

namespace PracticeMVC.Models.DTOs;
public class StudentModel
{
    public Guid? Id { get; set; }
    [Required, MinLength(1)]
    public string NRIC { get; set; }
    [Required, MinLength(1)]
    public string Name { get; set; }
    public GenderEnum GenderEnum { get; set; }
    [Required]
    public DateTime BirthDay { get; set; }
    public DateTime? AvailableDate { get; set; }
    public List<int>? SubjectValues { get; set; }
    public List<SubjectEnum>? Subjects { get; set; }
}