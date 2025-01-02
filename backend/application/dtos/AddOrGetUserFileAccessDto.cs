using System.ComponentModel.DataAnnotations;
using core.models;

namespace application.dtos;

public class AddOrGetUserFileAccessDto
{
    public string EncryptedFileKey { get; set; }
    public string Role { get; set; }
    public string UserId { get; set; }
    public string FileId { get; set; }
}