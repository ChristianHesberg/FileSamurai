namespace application.dtos;

public class GetFileDto
{
    public FileDto File { get; set; }
    public AddOrGetUserFileAccessDto UserFileAccess { get; set; }
}