using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public interface IStudentService
    {
        List<StudentDetailsDto> GetAllStudents();
        StudentDetailsDto? GetStudentById(int id);
        StudentDetailsDto AddStudent(CreateStudentDto newStudentDto);
        StudentDetailsDto? UpdateStudent(int id, UpdateStudentDto updatedStudentDto);
        bool DeleteStudent(int id);
        List<StudentDetailsDto> SearchByName(string name);
        List<StudentDetailsDto> FilterByAge();
    }
}
