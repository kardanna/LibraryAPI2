namespace LibraryAPI.Domain;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int PublishedYear { get; set; }
    public int AuthorId { get; set; }
    public virtual Author Author { get; set; } = null!;

    public Book Clone()
    {
        return new Book()
        {
            Id = Id,
            Title = Title,
            PublishedYear = PublishedYear,
            AuthorId = AuthorId,
            Author = new Author()
            {
                Id = Author.Id,
                Name = Author.Name,
                DateOfBirth = Author.DateOfBirth
            }
        };
    }
}