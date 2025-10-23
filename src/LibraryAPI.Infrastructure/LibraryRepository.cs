using LibraryAPI.Domain;
using System.Collections.Concurrent;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Application.Interfaces;

namespace LibraryAPI.Infrastructure;

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
            throw new RepositoryUpdateException(nameof(Author), RepositoryOperation.Add);
        }

        return entry.Id;
    }

    public void DeleteAuthor(int id)
    {
        if (!Authors.TryGetValue(id, out Author? author))
        {
            throw new EntityDoesNotExistException(nameof(Author), id);
        }

        lock (author.EntityLock)
        {
            if(!Authors.ContainsKey(id)) throw new EntityDoesNotExistException(nameof(Author), id);

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
                throw new RepositoryUpdateException(nameof(Book), RepositoryOperation.Delete);
            }

            if (!Authors.TryRemove(id, out _))
            {
                foreach (var book in removedBooks)
                {
                    Books.TryAdd(book.Value.Id, book.Value);
                }
                throw new RepositoryUpdateException(nameof(Author), RepositoryOperation.Delete);
            }
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
            throw new EntityDoesNotExistException(nameof(Author), id);
        }

        return value.Clone();
    }

    public void UpdateAuthor(Author entry)
    {
        if (!Authors.TryGetValue(entry.Id, out Author? existingEntry))
        {
            throw new EntityDoesNotExistException(nameof(Author), entry.Id);
        }

        lock (existingEntry.EntityLock)
        {
            if (!Authors.ContainsKey(entry.Id)) throw new EntityDoesNotExistException(nameof(Author), entry.Id);
            
            existingEntry.Name = entry.Name;
            existingEntry.DateOfBirth = entry.DateOfBirth;
        }
    }
    
    public int AddBook(Book entry)
    {
        if (!Authors.TryGetValue(entry.AuthorId, out Author? author))
        {
            throw new EntityDoesNotExistException(nameof(Author), entry.AuthorId);
        }

        lock (author.EntityLock)
        {
            if (!Authors.ContainsKey(entry.AuthorId)) throw new EntityDoesNotExistException(nameof(Author), entry.AuthorId);

            int id = Interlocked.Increment(ref _booksSequence);
            entry.Id = id;
            entry.Author = author;

            if (!Books.TryAdd(id, entry))
            {
                throw new RepositoryUpdateException(nameof(Book), RepositoryOperation.Add);
            }

            entry.Author.Books.Add(entry);

            return entry.Id;
        }
    }

    public void DeleteBook(int id)
    {
        if (!Books.TryGetValue(id, out Book? book))
        {
            throw new EntityDoesNotExistException(nameof(Book), id);
        }

        lock (book.Author.EntityLock)
        {
            if (!Books.ContainsKey(id)) throw new EntityDoesNotExistException(nameof(Book), id);

            if (!Books.TryRemove(id, out _))
            {
                throw new RepositoryUpdateException(nameof(Book), RepositoryOperation.Delete);
            }

            book.Author.Books.Remove(book);
        }
    }

    public IEnumerable<Book> GetAllBooks()
    {
        return Books.Values.Select(b => b.Clone());
    }

    public Book GetBook(int id)
    {
        if (!Books.TryGetValue(id, out Book? value))
        {
            throw new EntityDoesNotExistException(nameof(Book), id);
        }

        return value.Clone();
    }

    public void UpdateBook(Book entry)
    {
        if (!Books.TryGetValue(entry.Id, out Book? bookToUpdate))
        {
            throw new EntityDoesNotExistException(nameof(Book), entry.Id);
        }

        if (entry.AuthorId == bookToUpdate.AuthorId)
        {
            lock (bookToUpdate.Author.EntityLock)
            {
                if (!Books.ContainsKey(entry.Id)) throw new EntityDoesNotExistException(nameof(Book), entry.Id);

                bookToUpdate.Title = entry.Title;
                bookToUpdate.PublishedYear = entry.PublishedYear;
            }
        }
        else
        {
            if (!Authors.TryGetValue(entry.AuthorId, out Author? newAuthor))
            {
                throw new EntityDoesNotExistException(nameof(Author), entry.AuthorId);
            }

            var outerLock = bookToUpdate.AuthorId < newAuthor.Id ? bookToUpdate.Author.EntityLock : newAuthor.EntityLock;
            var innerLock = bookToUpdate.AuthorId < newAuthor.Id ? newAuthor.EntityLock : bookToUpdate.Author.EntityLock;

            lock (outerLock)
                lock (innerLock)
                {
                    if (!Books.ContainsKey(entry.Id)) throw new EntityDoesNotExistException(nameof(Book), entry.Id);
                    if (!Authors.ContainsKey(entry.AuthorId)) throw new EntityDoesNotExistException(nameof(Author), entry.AuthorId);

                    bookToUpdate.Author.Books.Remove(bookToUpdate);
                    bookToUpdate.Title = entry.Title;
                    bookToUpdate.PublishedYear = entry.PublishedYear;
                    bookToUpdate.AuthorId = entry.AuthorId;
                    bookToUpdate.Author = newAuthor;
                    bookToUpdate.Author.Books.Add(bookToUpdate);
                }
        }
    }
}