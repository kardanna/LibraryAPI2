namespace LibraryAPI.DTO.Book;

public class ReturnBookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int PublishedYear { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = null!;

    public ReturnBookDto(Models.Book book)
    {
        Id = book.Id;
        Title = book.Title;
        PublishedYear = book.PublishedYear;
        AuthorId = book.AuthorId;
        AuthorName = book.Author.Name;
    }
}