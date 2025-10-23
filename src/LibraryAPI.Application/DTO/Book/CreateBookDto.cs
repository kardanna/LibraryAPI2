namespace LibraryAPI.DTO.Book;

public class CreateBookDto
{
    public string Title { get; set; } = null!;
    public int PublishedYear { get; set; }
    public int AuthorId { get; set; }
}