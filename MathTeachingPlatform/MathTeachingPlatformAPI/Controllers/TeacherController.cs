using Application.DTOs.Teacher;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var teacher = await _teacherService.CreateTeacherAsync(request);
                return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, teacher);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherByIdAsync(id);
                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var teachers = await _teacherService.GetAllTeachersAsync();
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetTeacherByUserId(int userId)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherByUserIdAsync(userId);
                if (teacher == null)
                    return NotFound(new { error = "Teacher not found for this user" });

                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("by-department/{department}")]
        public async Task<IActionResult> GetTeachersByDepartment(string department)
        {
            try
            {
                var teachers = await _teacherService.GetTeachersByDepartmentAsync(department);
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] UpdateTeacherRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var teacher = await _teacherService.UpdateTeacherAsync(id, request);
                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            try
            {
                await _teacherService.DeleteTeacherAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/suspend")]
        public async Task<IActionResult> SuspendTeacher(int id)
        {
            try
            {
                await _teacherService.SuspendTeacherAsync(id);
                return Ok(new { message = "Teacher suspended successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateTeacher(int id)
        {
            try
            {
                await _teacherService.ActivateTeacherAsync(id);
                return Ok(new { message = "Teacher activated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}