using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.Model.Interfaces;

namespace ExaminationSystem.Model;


[Table("Student")]
public class Student: IEntityInt
{
  
    public int Id { get; set; }
    
    [Required]
    public  string Name { get; set; }
}