using LibraryAPI.Application.DTO.Author;

namespace LibraryAPI.Application.Services;

public interface IAuthorService
{
    public IEnumerable<ReturnAuthorDto> GetAllAuthors();
    public ReturnAuthorDto GetAuthor(int id);
    public ReturnAuthorDto AddAuthor(CreateAuthorDto entry);
    public void DeleteAuthor(int id);
    public void UpdateAuthor(int id, CreateAuthorDto entry);
}