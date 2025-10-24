namespace LibraryAPI2.Application.DTO.Author;

public class ReturnAuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public int BooksPublished { get; set; }

    public ReturnAuthorDto(Domain.Author author)
    {
        Id = author.Id;
        Name = author.Name;
        DateOfBirth = author.DateOfBirth;
        BooksPublished = author.Books.Count;
    }
}