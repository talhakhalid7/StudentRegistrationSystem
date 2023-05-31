using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class StudentModel
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public ICollection<RegistrationModel>? Registrations { get; set; }


    }
}
