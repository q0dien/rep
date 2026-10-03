using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private static readonly List<Student> Students = new()
    {
        new Student
        {
            Id = 1,
            FullName = "Айдана Сарсенова",
            Group = "CS-401",
            Course = 4,
            Email = "aidana@example.com"
        },
        new Student
        {
            Id = 2,
            FullName = "Данияр Ахметов",
            Group = "CS-402",
            Course = 4,
            Email = "daniyar@example.com"
        }
    };

    [HttpGet]
    public ActionResult<List<Student>> GetAll()
    {
        return Ok(Students);
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetById(int id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    public ActionResult<Student> Create(Student student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        student.Id = Students.Count == 0
            ? 1
            : Students.Max(s => s.Id) + 1;

        Students.Add(student);

        return CreatedAtAction(
            nameof(GetById),
            new { id = student.Id },
            student);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Student student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingStudent = Students.FirstOrDefault(s => s.Id == id);

        if (existingStudent == null)
        {
            return NotFound();
        }

        existingStudent.FullName = student.FullName;
        existingStudent.Group = student.Group;
        existingStudent.Course = student.Course;
        existingStudent.Email = student.Email;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);

        if (student == null)
        {
            return NotFound();
        }

        Students.Remove(student);

        return NoContent();
    }
}