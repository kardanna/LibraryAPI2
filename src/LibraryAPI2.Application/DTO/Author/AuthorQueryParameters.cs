using LibraryAPI2.Application.Common;

namespace LibraryAPI2.Application.DTO.Author;

public class AuthorQueryParameters
{
    public AuthorOrderBy? OrderBy { get; set; }
    public OrderDirection? OrderDirection { get; set; }
    public string? NameContains { get; set; }
    public string? NameStartsWith { get; set; }
}