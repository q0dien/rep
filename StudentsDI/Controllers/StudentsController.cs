using Microsoft.AspNetCore.Mvc;
using StudentsDI.Services;

namespace StudentsDI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        IStudentService studentService,
        ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            _logger.LogInformation("Getting all students");

            return Ok(_studentService.GetAll());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all students");

            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var student = _studentService.GetById(id);

            if (student == null)
            {
                _logger.LogWarning(
                    "Student with ID {StudentId} was not found",
                    id);

                return NotFound();
            }

            _logger.LogInformation(
                "Student with ID {StudentId} was found",
                id);

            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while processing student with ID {StudentId}",
                id);

            return StatusCode(500, "An error occurred while processing the request.");
        }
    }
}