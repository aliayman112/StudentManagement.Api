using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<StudentDetailsDto> ToStudentDetailsDto(Student student)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == student.DepartmentId);

            return new StudentDetailsDto
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age,
                DepartmentName = department != null ? department.Name : "Unknown"
            };
        }

        public async Task<List<StudentDetailsDto>> SearchByNameOrDepartment(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return await GetAllStudents();
            }

            var query = from student in _context.Students
                        join department in _context.Departments
                            on student.DepartmentId equals department.Id
                        where student.Name.Contains(text) || department.Name.Contains(text)
                        select new StudentDetailsDto
                        {
                            Id = student.Id,
                            Name = student.Name,
                            Age = student.Age,
                            DepartmentName = department.Name
                        };

            return await query.ToListAsync();
        }
        public async Task<List<StudentDetailsDto>> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            var result = new List<StudentDetailsDto>();
            foreach (var student in students)
            {
                result.Add(await ToStudentDetailsDto(student));
            }
            return result;
        }

        public async Task<StudentDetailsDto?> GetStudentById(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            return student == null ? null : await ToStudentDetailsDto(student);
        }

        public async Task<StudentDetailsDto> AddStudent(CreateStudentDto newStudentDto)
        {
            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == newStudentDto.DepartmentId);
            if (!departmentExists)
            {
                throw new ArgumentException($"Department with id {newStudentDto.DepartmentId} does not exist.");
            }

            var newStudent = new Student
            {
                Name = newStudentDto.Name,
                Age = newStudentDto.Age,
                DepartmentId = newStudentDto.DepartmentId
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            return await ToStudentDetailsDto(newStudent);
        }

        public async Task<StudentDetailsDto?> UpdateStudent(int id, UpdateStudentDto updatedStudentDto)
        {
            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStudent == null)
            {
                return null;
            }

            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == updatedStudentDto.DepartmentId);
            if (!departmentExists)
            {
                throw new ArgumentException($"Department with id {updatedStudentDto.DepartmentId} does not exist.");
            }

            existingStudent.Name = updatedStudentDto.Name;
            existingStudent.Age = updatedStudentDto.Age;
            existingStudent.DepartmentId = updatedStudentDto.DepartmentId;

            await _context.SaveChangesAsync();

            return await ToStudentDetailsDto(existingStudent);
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentDetailsDto>> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await GetAllStudents();
            }

            var students = await _context.Students
                .Where(s => s.Name.Contains(name))
                .ToListAsync();

            var result = new List<StudentDetailsDto>();
            foreach (var student in students)
            {
                result.Add(await ToStudentDetailsDto(student));
            }
            return result;
        }

        public async Task<List<StudentDetailsDto>> FilterByAge()
        {
            var students = await _context.Students
                .Where(s => s.Age >= 18 && s.Age <= 22)
                .OrderBy(s => s.Age)
                .ToListAsync();

            var result = new List<StudentDetailsDto>();
            foreach (var student in students)
            {
                result.Add(await ToStudentDetailsDto(student));
            }
            return result;
        }
    }
}