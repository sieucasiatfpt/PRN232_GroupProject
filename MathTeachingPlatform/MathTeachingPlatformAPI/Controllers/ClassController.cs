using Application.DTOs.Class;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("classes")] // Removed the /api prefix
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] CreateClassRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var classDto = await _classService.CreateClassAsync(request);
                return CreatedAtAction(nameof(GetClassById), new { id = classDto.ClassId }, classDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        //[Authorize(Roles = "Teacher,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            try
            {
                var classDto = await _classService.GetClassByIdAsync(id);
                return Ok(classDto);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAllClasses()
        {
            try
            {
                var classes = await _classService.GetAllClassesAsync();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        //[Authorize(Roles = "Teacher,Admin")]
        [HttpGet("by-teacher/{teacherId}")]
        public async Task<IActionResult> GetClassesByTeacherId(int teacherId)
        {
            try
            {
                var classes = await _classService.GetClassesByTeacherIdAsync(teacherId);
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        //[Authorize(Roles = "Teacher,Admin")]
        [HttpGet("by-subject/{subjectId}")]
        public async Task<IActionResult> GetClassesBySubjectId(int subjectId)
        {
            try
            {
                var classes = await _classService.GetClassesBySubjectIdAsync(subjectId);
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] UpdateClassRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var classDto = await _classService.UpdateClassAsync(id, request);
                return Ok(classDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                await _classService.DeleteClassAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin,Student")]
        [HttpPost("{classId}/enrollment/{studentId}")]
        public async Task<IActionResult> EnrollStudent(int classId, int studentId)
        {
            try
            {
                await _classService.EnrollStudentAsync(classId, studentId);
                return Ok(new { message = "Student enrolled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpDelete("{classId}/unenrollment/{studentId}")]
        public async Task<IActionResult> UnenrollStudent(int classId, int studentId)
        {
            try
            {
                await _classService.UnenrollStudentAsync(classId, studentId);
                return Ok(new { message = "Student unenrolled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("teachers/{teacherId}/active-classes")]
        public async Task<IActionResult> HasActiveClassesAsync(int teacherId)
        {
            var hasActiveClasses = await _classService.HasActiveClassesAsync(teacherId);
            return Ok(hasActiveClasses);
        }
    }
}