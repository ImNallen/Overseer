namespace Overseer.Api.Services.Encryption;

public interface IPasswordGenerator
{
    string Generate(int length = 16);
}
