using System.Security.Cryptography;
using Overseer.Api.Abstractions.Encryption;

namespace Overseer.Api.Services.Authentication;

public class PasswordGenerator : IPasswordGenerator
{
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NumberChars = "0123456789";
    private const string SpecialChars = "!@#$%^&*()_-+=<>?";

    public string Generate(int length = 16)
    {
        const string allChars = LowercaseChars + UppercaseChars + NumberChars + SpecialChars;

        if (length < 8)
        {
            throw new ArgumentException("Password length must be at least 8 characters.", nameof(length));
        }

        char[] password = new char[length];

        // Ensure at least one character from each category
        password[0] = GetRandomChar(LowercaseChars);
        password[1] = GetRandomChar(UppercaseChars);
        password[2] = GetRandomChar(NumberChars);
        password[3] = GetRandomChar(SpecialChars);

        // Fill the rest with random characters from all categories
        for (int i = 4; i < length; i++)
        {
            password[i] = GetRandomChar(allChars);
        }

        // Shuffle the password characters
        ShuffleArray(password);

        return new string(password);
    }

    private static char GetRandomChar(string characterSet) => characterSet[RandomNumberGenerator.GetInt32(characterSet.Length)];

    private static void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = RandomNumberGenerator.GetInt32(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
