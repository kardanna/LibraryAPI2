namespace LibraryAPI2.Application.DTO.Author;

public class CreateAuthorDto
{
    public string Name { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
}