using core.models;
using core.ports;

namespace shared.adapters;

public class UserDataSqliteAdapter(Context context) : IUserDataPort
{
    public void AddUserKeyPair(UserRsaKeyPair userRsaKeyPair)
    {
        context.UserRsaKeyPairs.Add(userRsaKeyPair);
        context.SaveChanges();
    }
}