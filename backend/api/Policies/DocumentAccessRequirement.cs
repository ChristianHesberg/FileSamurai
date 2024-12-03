using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class DocumentAccessRequirement(string documentId) : IAuthorizationRequirement
{
    public string DocumentId { get; } = documentId;
}