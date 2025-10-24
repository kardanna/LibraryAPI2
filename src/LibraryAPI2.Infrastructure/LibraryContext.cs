using Microsoft.EntityFrameworkCore;
using LibraryAPI2.Domain;

namespace LibraryAPI2.Infrastructure;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
}