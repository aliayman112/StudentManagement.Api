using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private static readonly List<Department> _departments = new()
        {
            new Department { Id = 1, Name = "IT" },
            new Department { Id = 2, Name = "HR" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "Sales" }
        };

        private readonly IStudentService _studentService;

        public DepartmentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("statistics")]
        public IActionResult GetDepartmentStatistics()
        {
            var students = _studentService.GetAllStudents();

            var statistics = students
                .GroupBy(s => s.DepartmentName)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    NumberOfStudents = g.Count(),
                    AverageAge = g.Average(s => s.Age),
                    OldestAge = g.Max(s => s.Age),
                    YoungestAge = g.Min(s => s.Age)
                })
                .ToList();

            return Ok(statistics);
        }

        [HttpGet("highest-lowest")]
        public IActionResult GetHighestAndLowestDepartment()
        {
            var students = _studentService.GetAllStudents();

            var statistics = students
                .GroupBy(s => s.DepartmentName)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    NumberOfStudents = g.Count()
                })
                .ToList();

            if (!statistics.Any())
            {
                return Ok(new { Message = "No students found." });
            }

            var highest = statistics.OrderByDescending(s => s.NumberOfStudents).First();
            var lowest = statistics.OrderBy(s => s.NumberOfStudents).First();

            var result = new
            {
                Highest = highest,
                Lowest = lowest
            };

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            return Ok(_departments);
        }

        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(int id)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost]
        public IActionResult AddDepartment([FromBody] Department newDepartment)
        {
            newDepartment.Id = _departments.Any() ? _departments.Max(d => d.Id) + 1 : 1;
            _departments.Add(newDepartment);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = newDepartment.Id }, newDepartment);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            var existingDepartment = _departments.FirstOrDefault(d => d.Id == id);
            if (existingDepartment == null)
            {
                return NotFound();
            }
            existingDepartment.Name = updatedDepartment.Name;
            return Ok(existingDepartment);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            _departments.Remove(department);
            return NoContent();
        }
    }
}
