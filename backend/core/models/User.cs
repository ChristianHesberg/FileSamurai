namespace core.models;

public class User
{
    public string Id { get; set; }
    public UserRsaKeyPair UserRsaKeyPair { get; set; }
    //public List<UserFileAccess> UserFileAccesses { get; set; }
    //public List<UserGroupAccess> UserGroupAccess { get; set; }
}