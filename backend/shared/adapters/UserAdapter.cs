using application.ports;
using core.models;

namespace shared.adapters;

public class UserAdapter(Context context) : IUserPort
{
    public void AddUserKeyPair(UserRsaKeyPair userRsaKeyPair)
    {
        var res = context.UserRsaKeyPairs.Add(userRsaKeyPair);
        context.SaveChanges();
    }

    public UserRsaKeyPair? GetUserRsaKeyPair(string userId)
    {
        return context.UserRsaKeyPairs.FirstOrDefault(r => r.Id == userId);
    }

    public string? GetUserPublicKey(string userId)
    {
        var entity = context.UserRsaKeyPairs.FirstOrDefault(r => r.Id == userId);
        if (entity == null) return null;
        return entity.PublicKey;
    }
}