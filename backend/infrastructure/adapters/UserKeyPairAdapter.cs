using application.ports;
using core.models;

namespace infrastructure.adapters;

public class UserKeyPairAdapter(Context context) : IUserKeyPairPort
{
    public void AddUserKeyPair(UserRsaKeyPair userRsaKeyPair)
    {
        context.UserRsaKeyPairs.Add(userRsaKeyPair);
        context.SaveChanges();
    }

    public UserRsaKeyPair GetUserRsaKeyPair(string userId)
    {
        var res = context.UserRsaKeyPairs.FirstOrDefault(pair => pair.UserId == userId);
        if (res == null) throw new KeyNotFoundException("No key pair found for user");
        return res;
    }

    public string GetUserPublicKey(string userId)
    {
        var entity = context.UserRsaKeyPairs.FirstOrDefault(pair => pair.UserId == userId);
        if (entity == null) throw new KeyNotFoundException("No key pair found for user");
        return entity.PublicKey;
    }
}