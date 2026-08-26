using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public interface IStudentService
    {
        Task<List<StudentDetailsDto>> GetAllStudents();
        Task<StudentDetailsDto?> GetStudentById(int id);
        Task<StudentDetailsDto> AddStudent(CreateStudentDto newStudentDto);
        Task<StudentDetailsDto?> UpdateStudent(int id, UpdateStudentDto updatedStudentDto);
        Task<bool> DeleteStudent(int id);
        Task<List<StudentDetailsDto>> SearchByName(string name);
        Task<List<StudentDetailsDto>> FilterByAge();
        Task<List<StudentDetailsDto>> SearchByNameOrDepartment(string text);
    }
}
