namespace StudentManagement.Api.Dtos
{
    public class CreateStudentDto
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public int DepartmentId { get; set; }
    }
}

