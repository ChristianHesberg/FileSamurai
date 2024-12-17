using application.ports;
using core.models;

namespace infrastructure.adapters;

public class UserKeyPairKeyPairAdapter(Context context) : IUserKeyPairPort
{
    public void AddUserKeyPair(UserRsaKeyPair userRsaKeyPair)
    {
        context.UserRsaKeyPairs.Add(userRsaKeyPair);
        context.SaveChanges();
    }

    public UserRsaKeyPair? GetUserRsaKeyPair(string userId)
    {
        return context.UserRsaKeyPairs.Find(userId);
    }

    public string? GetUserPublicKey(string userId)
    {
        var entity = context.UserRsaKeyPairs.FirstOrDefault(pair => pair.UserId == userId);
        if (entity == null) return null;
        return entity.PublicKey;
    }
}