using LibraryAPI2.Application.Common;

namespace LibraryAPI2.Application.DTO.Book;

public class BookQueryParameters
{
    public BookOrderBy? OrderBy { get; set; }
    public OrderDirection? OrderDirection { get; set; }
    public int? PublishedBefore { get; set; }
    public int? PublishedAfter { get; set; }
}