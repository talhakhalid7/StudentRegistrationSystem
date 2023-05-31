using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class CourseModel
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailableSpaces { get; set; }

        public ICollection<RegistrationModel>? Registrations { get; set; }
    }
}
