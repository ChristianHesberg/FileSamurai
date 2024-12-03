using core.models;
using core.ports;

namespace shared.adapters;

public class UserAdapter(Context context) : IUserPort
{
    public UserRsaKeyPair AddUserKeyPair(UserRsaKeyPair userRsaKeyPair)
    {
        var res = context.UserRsaKeyPairs.Add(userRsaKeyPair);
        context.SaveChanges();
        return res.Entity;
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