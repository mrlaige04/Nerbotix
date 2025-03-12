using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RoboTasker.Infrastructure.Authentication.Providers;

public class CodeConfirmationTokenProvider<TUser>(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<DataProtectionTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger)
    : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger)
    where TUser : class
{
    private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public override Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var token = new char[24];
        var rng = RandomNumberGenerator.Create();
        
        for (int i = 0; i < token.Length; i++)
        {
            byte[] randomByte = new byte[1];
            rng.GetBytes(randomByte);
            token[i] = AllowedChars[randomByte[0] % AllowedChars.Length];
        }
        
        return Task.FromResult(new string(token));
    }
    
    public override Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        return Task.FromResult(token.Length == 24 && token.All(c => AllowedChars.Contains(c)));
    }
}