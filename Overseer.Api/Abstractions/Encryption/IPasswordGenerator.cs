namespace Overseer.Api.Abstractions.Encryption;

public interface IPasswordGenerator
{
    string Generate(int length = 16);
}
