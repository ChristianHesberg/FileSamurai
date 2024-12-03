using System.ComponentModel.DataAnnotations;

namespace core.models;

public class Group
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; }
}