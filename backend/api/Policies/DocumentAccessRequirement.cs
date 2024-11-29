using Microsoft.AspNetCore.Authorization;

namespace api.Policies;

public class DocumentAccessRequirement(int documentId) : IAuthorizationRequirement
{
    public int DocumentId { get; } = documentId;
}