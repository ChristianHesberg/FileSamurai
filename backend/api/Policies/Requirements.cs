using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public abstract class Requirements
{
    public class KeyPairPostRequirement : IAuthorizationRequirement { }
    public class KeyPairGetPrivateKeyRequirement : IAuthorizationRequirement{ }
    public class GroupAddUserRequirement : IAuthorizationRequirement { }
    public class DocumentGetUserFileAccessRequirement : IAuthorizationRequirement { } 
    public class DocumentGetRequirement : IAuthorizationRequirement { }
    public class DocumentChangeRequirement() : IAuthorizationRequirement { } 
    public class DocumentAddRequirement : IAuthorizationRequirement { }
    public class DocumentAccessRequirement() : IAuthorizationRequirement { }
    
    public class GroupDeleteRequirement() : IAuthorizationRequirement { }
    
    public class GroupGetRequirement() : IAuthorizationRequirement { }
}