using application.ports;
using core.errors;
using core.models;

namespace infrastructure.adapters;

public class UserKeyPairAdapter(Context context) : IUserKeyPairPort
{
    public void AddUserKeyPair(UserRsaKeyPair userRsaKeyPair)
    {
        try
        {
            context.UserRsaKeyPairs.Add(userRsaKeyPair);
            context.SaveChanges();
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
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

    public List<RsaPublicKeyWithId> GetPublicKeys(string[] idList)
    {
        var keyList = context.UserRsaKeyPairs.Where(key => idList.Contains(key.UserId)).ToList();
        if (keyList.Count != idList.Length)
            throw new KeyNotFoundException(
                "One or more ids did not exist in database. Please check provided ids and try again.");
        return keyList.Select(keypair => new RsaPublicKeyWithId()
        {
            UserId = keypair.UserId,
            PublicKey = keypair.PublicKey
        }).ToList();
    }
}