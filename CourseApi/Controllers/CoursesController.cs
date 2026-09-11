using CourseApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private static List<Course> courses = new List<Course>
    {
        new Course
        {
            Id = 1,
            Name = "Web Services",
            Teacher = "Teacher 1",
            Credits = 5
        },
        new Course
        {
            Id = 2,
            Name = "Databases",
            Teacher = "Teacher 2",
            Credits = 4
        }
    };

    [HttpGet]
    public ActionResult<List<Course>> GetAll()
    {
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public ActionResult<Course> GetById(int id)
    {
        var course = courses.FirstOrDefault(x => x.Id == id);

        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    public ActionResult<Course> Create(Course course)
    {
        course.Id = courses.Max(x => x.Id) + 1;
        courses.Add(course);

        return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public ActionResult<Course> Update(int id, Course course)
    {
        var oldCourse = courses.FirstOrDefault(x => x.Id == id);

        if (oldCourse == null)
        {
            return NotFound();
        }

        oldCourse.Name = course.Name;
        oldCourse.Teacher = course.Teacher;
        oldCourse.Credits = course.Credits;

        return Ok(oldCourse);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var course = courses.FirstOrDefault(x => x.Id == id);

        if (course == null)
        {
            return NotFound();
        }

        courses.Remove(course);

        return NoContent();
    }
}