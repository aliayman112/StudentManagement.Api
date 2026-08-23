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
        public IActionResult GetAllStudents()
        {
            return Ok(_studentService.GetAllStudents());
        }

        [HttpGet("search")]
        public IActionResult SearchByName([FromQuery] string name)
        {
            return Ok(_studentService.SearchByName(name));
        }

        [HttpGet("filter-by-age")]
        public IActionResult FilterByAge()
        {
            return Ok(_studentService.FilterByAge());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] CreateStudentDto newStudentDto)
        {
            try
            {
                var created = _studentService.AddStudent(newStudentDto);
                return CreatedAtAction(nameof(GetStudentById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] UpdateStudentDto updatedStudentDto)
        {
            try
            {
                var updated = _studentService.UpdateStudent(id, updatedStudentDto);
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
        public IActionResult DeleteStudent(int id)
        {
            var deleted = _studentService.DeleteStudent(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
