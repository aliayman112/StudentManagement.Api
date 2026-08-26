using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IStudentService _studentService;

        public DepartmentsController(IDepartmentService departmentService, IStudentService studentService)
        {
            _departmentService = departmentService;
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            return Ok(await _departmentService.GetAllDepartments());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] Department newDepartment)
        {
            try
            {
                var created = await _departmentService.AddDepartment(newDepartment);
                return CreatedAtAction(nameof(GetDepartmentById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            try
            {
                var updated = await _departmentService.UpdateDepartment(id, updatedDepartment);
                if (updated == null)
                {
                    return NotFound();
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var deleted = await _departmentService.DeleteDepartment(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetDepartmentStatistics()
        {
            var students = await _studentService.GetAllStudents();

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
        public async Task<IActionResult> GetHighestAndLowestDepartment()
        {
            var students = await _studentService.GetAllStudents();

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

            var highestCount = statistics.Max(s => s.NumberOfStudents);
            var lowestCount = statistics.Min(s => s.NumberOfStudents);

            var highestDepartments = statistics.Where(s => s.NumberOfStudents == highestCount).ToList();
            var lowestDepartments = statistics.Where(s => s.NumberOfStudents == lowestCount).ToList();

            return Ok(new
            {
                Highest = highestDepartments,
                Lowest = lowestDepartments
            });
        }
    }
}
