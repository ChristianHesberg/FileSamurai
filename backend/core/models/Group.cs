using System.ComponentModel.DataAnnotations;

namespace core.models;

public class Group
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string SharingId { get; set; } = Guid.NewGuid().ToString();
    public List<User> Users { get; set; }
}