using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class RegistrationModel
    {
        [Key]
        public int RegistrationId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public StudentModel? Student { get; set; }
        public CourseModel? Course { get; set; }
    }
}
