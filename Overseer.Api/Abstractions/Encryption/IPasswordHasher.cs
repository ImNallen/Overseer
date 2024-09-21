﻿namespace Overseer.Api.Abstractions.Encryption;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string hashedPassword);
}
