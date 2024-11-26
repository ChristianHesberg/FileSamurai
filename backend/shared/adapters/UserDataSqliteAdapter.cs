﻿using core.models;
using core.ports;

namespace shared.adapters;

public class UserDataSqliteAdapter(Context context) : IUserDataPort
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
}