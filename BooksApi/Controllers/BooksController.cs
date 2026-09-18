using Microsoft.AspNetCore.Mvc;
using BooksApi.Models;

namespace BooksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private static List<Book> books = new()
    {
        new Book
        {
            Id = 1,
            Title = "Мастер и Маргарита",
            Author = "Михаил Булгаков",
            Year = 1967
        },
        new Book
        {
            Id = 2,
            Title = "Преступление и наказание",
            Author = "Фёдор Достоевский",
            Year = 1866
        },
        new Book
        {
            Id = 3,
            Title = "Война и мир",
            Author = "Лев Толстой",
            Year = 1869
        }
    };

    [HttpGet]
    public ActionResult<List<Book>> GetBooks()
    {
        return Ok(books);
    }

    [HttpPost]
    public ActionResult<Book> AddBook(Book book)
    {
        books.Add(book);

        return Ok(book);
    }
}