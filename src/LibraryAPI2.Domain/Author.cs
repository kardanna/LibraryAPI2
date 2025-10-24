namespace LibraryAPI2.Domain;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public readonly object EntityLock = new();

    public Author Clone()
    {
        return new Author()
        {
            Id = Id,
            Name = Name,
            DateOfBirth = DateOfBirth,
            Books = Books.Select(b => b.Clone()).ToList()
        };
    }
}