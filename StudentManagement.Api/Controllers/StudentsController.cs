using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Services;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            return Ok(await _studentService.GetAllStudents());
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByNameOrDepartment([FromQuery] string text)
        {
            return Ok(await _studentService.SearchByNameOrDepartment(text));
        }

        [HttpGet("filter-by-age")]
        public async Task<IActionResult> FilterByAge()
        {
            return Ok(await _studentService.FilterByAge());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] CreateStudentDto newStudentDto)
        {
            try
            {
                var created = await _studentService.AddStudent(newStudentDto);
                return CreatedAtAction(nameof(GetStudentById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto updatedStudentDto)
        {
            try
            {
                var updated = await _studentService.UpdateStudent(id, updatedStudentDto);
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
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var deleted = await _studentService.DeleteStudent(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}