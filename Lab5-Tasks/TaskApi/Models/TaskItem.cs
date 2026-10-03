using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название задачи")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите описание задачи")]
    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }
}