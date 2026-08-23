using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    public class StudentService : IStudentService
    {
        private static readonly List<Department> _departments = new()
        {
            new Department { Id = 1, Name = "IT" },
            new Department { Id = 2, Name = "HR" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "Sales" }
        };

        private static readonly List<Student> _students = new()
        {
            new Student { Id = 1, Name = "Ali Ayman", Age = 20, DepartmentId = 1 },
            new Student { Id = 2, Name = "Anwar el sadat", Age = 22, DepartmentId = 2 },
            new Student { Id = 3, Name = "Hamdy el merghany", Age = 21, DepartmentId = 3 },
            new Student { Id = 4, Name = "Ronald Araujo", Age = 17, DepartmentId = 1 },
            new Student { Id = 5, Name = "Adolf Kitler", Age = 23, DepartmentId = 4 }
        };

        private StudentDetailsDto ToStudentDetailsDto(Student student)
        {
            var department = _departments.FirstOrDefault(d => d.Id == student.DepartmentId);

            return new StudentDetailsDto
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age,
                DepartmentName = department != null ? department.Name : "Unknown"
            };
        }

        public List<StudentDetailsDto> GetAllStudents()
        {
            return _students.Select(s => ToStudentDetailsDto(s)).ToList();
        }

        public StudentDetailsDto? GetStudentById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            return student == null ? null : ToStudentDetailsDto(student);
        }

        public StudentDetailsDto AddStudent(CreateStudentDto newStudentDto)
        {
            var departmentExists = _departments.Any(d => d.Id == newStudentDto.DepartmentId);
            if (!departmentExists)
            {
                throw new ArgumentException($"Department with id {newStudentDto.DepartmentId} does not exist.");
            }

            var newStudent = new Student
            {
                Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1,
                Name = newStudentDto.Name,
                Age = newStudentDto.Age,
                DepartmentId = newStudentDto.DepartmentId
            };

            _students.Add(newStudent);
            return ToStudentDetailsDto(newStudent);
        }

        public StudentDetailsDto? UpdateStudent(int id, UpdateStudentDto updatedStudentDto)
        {
            var existingStudent = _students.FirstOrDefault(s => s.Id == id);
            if (existingStudent == null)
            {
                return null;
            }

            var departmentExists = _departments.Any(d => d.Id == updatedStudentDto.DepartmentId);
            if (!departmentExists)
            {
                throw new ArgumentException($"Department with id {updatedStudentDto.DepartmentId} does not exist.");
            }

            existingStudent.Name = updatedStudentDto.Name;
            existingStudent.Age = updatedStudentDto.Age;
            existingStudent.DepartmentId = updatedStudentDto.DepartmentId;

            return ToStudentDetailsDto(existingStudent);
        }

        public bool DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return false;
            }

            _students.Remove(student);
            return true;
        }

        public List<StudentDetailsDto> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return GetAllStudents();
            }

            return _students
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(s => ToStudentDetailsDto(s))
                .ToList();
        }

        public List<StudentDetailsDto> FilterByAge()
        {
            return _students
                .Where(s => s.Age >= 18 && s.Age <= 22)
                .OrderBy(s => s.Age)
                .Select(s => ToStudentDetailsDto(s))
                .ToList();
        }
    }
}