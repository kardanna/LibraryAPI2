using LibraryAPI2.Application.DTO.Author;
using LibraryAPI2.Application.Common;

namespace LibraryAPI2.Application.Services;

public interface IAuthorService
{
    public Task<IEnumerable<ReturnAuthorDto>> GetAllAuthors(AuthorQueryParameters? query);
    public Task<ReturnAuthorDto> GetAuthor(int id);
    public Task<ReturnAuthorDto> AddAuthor(CreateAuthorDto entry);
    public Task DeleteAuthor(int id);
    public Task UpdateAuthor(int id, CreateAuthorDto entry);
}