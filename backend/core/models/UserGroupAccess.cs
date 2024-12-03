using System.ComponentModel.DataAnnotations.Schema;

namespace core.models;

public class UserGroupAccess
{
    //needs to be composite key
    [ForeignKey("User")]
    public string UserId { get; set; }
    [ForeignKey("Group")] 
    public string GroupId { get; set; }
}