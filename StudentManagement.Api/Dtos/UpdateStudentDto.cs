using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Dtos
{
    public class UpdateStudentDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60.")]
        public int Age { get; set; }

        public int DepartmentId { get; set; }
    }
}