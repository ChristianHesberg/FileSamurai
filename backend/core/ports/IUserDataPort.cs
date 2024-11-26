﻿using core.models;

namespace core.ports;

public interface IUserDataPort
{
    public void AddUserKeyPair(UserRsaKeyPair userRsaKeyPair);
    public UserRsaKeyPair? GetUserRsaKeyPair(string userId);
}