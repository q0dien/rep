
namespace CourseApi.Models;

public class Teacher
{
    public int Id { get; set; }

    // ID учётной записи ASP.NET Identity
    public string UserId { get; set; } = "";

    public string FullName { get; set; } = "";
}
