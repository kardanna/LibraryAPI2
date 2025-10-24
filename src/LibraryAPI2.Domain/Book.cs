namespace LibraryAPI2.Domain;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int PublishedYear { get; set; }
    public int AuthorId { get; set; }
    public virtual Author Author { get; set; } = null!;
}