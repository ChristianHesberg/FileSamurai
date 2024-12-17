namespace application.dtos;

public class AddFileDto
{
    public string FileContents { get; set; }
    public string Nonce { get; set; }
    public string Tag { get; set; }
    public string Title { get; set; }
    public string GroupId { get; set; }
}