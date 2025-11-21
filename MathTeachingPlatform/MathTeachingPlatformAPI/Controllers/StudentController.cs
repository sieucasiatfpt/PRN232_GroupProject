using Application.DTOs.Student;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("student")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            return userId;
        }

        [Authorize(Roles = "Student")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = GetCurrentUserId();
                var student = await _studentService.GetStudentByUserIdAsync(userId);

                if (student == null)
                    return NotFound(new { error = "Student profile not found" });

                return Ok(student);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                    return NotFound(new { error = "Student not found" });

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var student = await _studentService.CreateStudentAsync(request);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var student = await _studentService.UpdateStudentAsync(id, request);
                if (student == null)
                    return NotFound(new { error = "Student not found" });

                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStudentStatus(int id, [FromBody] UpdateStudentStatusRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var student = await _studentService.UpdateStudentStatusAsync(id, request.Status);
                if (student == null)
                    return NotFound(new { error = "Student not found" });

                var action = request.Status == Domain.Enum.StudentStatus.Suspended ? "suspended" : "activated";
                return Ok(new
                {
                    message = $"Student account has been {action}",
                    student
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost("{id}/suspension")]
        public async Task<IActionResult> SuspendStudent(int id)
        {
            try
            {
                await _studentService.SuspendStudentAsync(id);
                return Ok(new { message = "Student suspended successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost("{id}/activation")]
        public async Task<IActionResult> ActivateStudent(int id)
        {
            try
            {
                await _studentService.ActivateStudentAsync(id);
                return Ok(new { message = "Student activated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
