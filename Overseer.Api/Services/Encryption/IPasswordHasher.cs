namespace Overseer.Api.Services.Encryption;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string hashedPassword);
}
