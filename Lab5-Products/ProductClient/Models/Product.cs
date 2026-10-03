using System.ComponentModel.DataAnnotations;

namespace ProductClient.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название товара")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите описание товара")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1000000000, ErrorMessage = "Цена должна быть больше 0")]
    public decimal Price { get; set; }

    [Range(0, 1000000, ErrorMessage = "Количество не может быть отрицательным")]
    public int Quantity { get; set; }
}