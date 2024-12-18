namespace application.dtos;

public class FileDto
{
    public string Id { get; set; }
    public string FileContents { get; set; }
    public string Nonce { get; set; }
    public string Tag { get; set; }
    public string Title { get; set; }
}