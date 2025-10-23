using LibraryAPI.Domain;
using System.Collections.Concurrent;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Data;

public class LibraryRepository : ILibraryRepository
{
    private readonly ConcurrentDictionary<int, Book> Books = new();
    private int _booksSequence = 0;
    private readonly ConcurrentDictionary<int, Author> Authors = new();
    private int _authorsSequence = 0;

    public int AddAuthor(Author entry)
    {
        int id = Interlocked.Increment(ref _authorsSequence);
        entry.Id = id;

        if (!Authors.TryAdd(id, entry))
        {
            throw new RepositoryUpdateException($"Failed to add author '{entry.Name}'");
        }

        return entry.Id;
    }

    public void DeleteAuthor(int id)
    {
        if (!Authors.ContainsKey(id)) throw new ArgumentException($"Author entry with ID {id} does not exist");

        var removedBooks = new Dictionary<int, Book>();
        var bookIdsToRemove = Books.Where(b => b.Value.AuthorId == id).Select(b => b.Value.Id);
        
        foreach (var bookId in bookIdsToRemove)
        {
            if (Books.TryRemove(bookId, out Book? removedBook))
            {
                removedBooks.Add(removedBook.Id, removedBook);
                continue;
            }

            foreach (var book in removedBooks)
            {
                Books.TryAdd(book.Value.Id, book.Value);
            }
            throw new RepositoryUpdateException($"Failed to performe cascade delete of all books by author with ID {id}");
        }
        
        if (!Authors.TryRemove(id, out _))
        {
            foreach (var book in removedBooks)
            {
                Books.TryAdd(book.Value.Id, book.Value);
            }
            throw new RepositoryUpdateException($"Failed to delete author with ID {id}");
        }
    }

    public IEnumerable<Author> GetAllAuthors()
    {
        return Authors.Values.Select(a => a.Clone());
    }

    public Author GetAuthor(int id)
    {
        if (!Authors.TryGetValue(id, out Author? value))
        {
            throw new ArgumentException($"Failed to retrieve an author with ID {id}");
        }

        return value.Clone();
    }

    public void UpdateAuthor(Author entry)
    {
        if (!Authors.TryGetValue(entry.Id, out Author? existingEntry))
        {
            throw new ArgumentException($"Author entry with ID {entry.Id} does not exist");
        }

        lock (existingEntry)
        {
            existingEntry.Name = entry.Name;
            existingEntry.DateOfBirth = entry.DateOfBirth;
        }
    }
    
    public int AddBook(Book entry)
    {
        if (!Authors.TryGetValue(entry.AuthorId, out Author? author))
        {
            throw new ArgumentException($"No author with ID {entry.AuthorId}");
        }

        int id = Interlocked.Increment(ref _booksSequence);
        entry.Id = id;
        entry.Author = author;

        if (Books.TryAdd(id, entry))
        {
            lock (entry.Author.Books)
            {
                entry.Author.Books.Add(entry);
            }
            return entry.Id;
        }
        
        throw new RepositoryUpdateException($"Failed to add book '{entry.Title}'");
    }

    public void DeleteBook(int id)
    {
        if (!Books.ContainsKey(id)) throw new ArgumentException($"Author entry with ID {id} does not exist");;

        if (Books.TryRemove(id, out Book? book))
        {
            lock (book.Author.Books)
            {
                book.Author.Books.Remove(book);
            }
            return;
        }
        
        throw new RepositoryUpdateException($"Failed to delete book with ID {id}");
    }

    public IEnumerable<Book> GetAllBooks()
    {
        return Books.Values.Select(b => b.Clone());
    }

    public Book GetBook(int id)
    {
        if (!Books.TryGetValue(id, out Book? value))
        {
            throw new ArgumentException($"Failed to retrieve a book with ID {id}");
        }

        return value.Clone();
    }

    public void UpdateBook(Book entry)
    {
        if (!Authors.TryGetValue(entry.AuthorId, out Author? author))
        {
            throw new ArgumentException($"No author with ID {entry.AuthorId}");
        }

        if (!Books.TryGetValue(entry.Id, out Book? existingEntry))
        {
            throw new ArgumentException($"Book entry with ID {entry.Id} does not exist");
        }

        lock (existingEntry)
        {
            existingEntry.Title = entry.Title;
            existingEntry.PublishedYear = entry.PublishedYear;
            existingEntry.AuthorId = entry.AuthorId;
            existingEntry.Author = author;
        }
    }
}