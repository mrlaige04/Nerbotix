using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RoboTasker.Infrastructure.Authentication.Providers;

public class NumericEmailTokenProvider<TUser>(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<DataProtectionTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger)
    : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger)
    where TUser : class
{
    public override Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var randomNumber = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        return Task.FromResult(randomNumber);
    }
    
    public override Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        return Task.FromResult(token.Length == 6 && token.All(char.IsDigit));
    }
}