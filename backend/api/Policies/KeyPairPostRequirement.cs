using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class KeyPairPostRequirement() : IAuthorizationRequirement
{
}