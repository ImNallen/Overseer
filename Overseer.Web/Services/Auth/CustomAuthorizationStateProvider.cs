﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Overseer.Web.Services.Auth;

public class CustomAuthorizationStateProvider(
    ILocalStorageService localStorageService)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        string? accessToken = await localStorageService.GetItemAsStringAsync("accessToken");

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var token = new JwtSecurityToken(accessToken.Replace("\"", string.Empty, StringComparison.InvariantCulture));

            if (IsTokenExpired(token))
            {
                await LogoutUser();
            }
            else
            {
                try
                {
                    identity = new ClaimsIdentity(
                        [
                        new Claim(ClaimTypes.Email, token.Claims.First(c => c.Type == "email").Value),
                        new Claim("unique_name", token.Claims.First(c => c.Type == "unique_name").Value),
                        new Claim(ClaimTypes.GivenName, token.Claims.First(c => c.Type == "given_name").Value),
                        new Claim(ClaimTypes.Surname, token.Claims.First(c => c.Type == "family_name").Value),
                        new Claim(ClaimTypes.Role, token.Claims.First(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value),
                        new Claim("access_token", accessToken)
                    ],
                        "jwt");
                }
                catch (Exception)
                {
                    await LogoutUser();
                }
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private static bool IsTokenExpired(JwtSecurityToken token) => token.ValidTo < DateTime.UtcNow;

    private async Task LogoutUser()
    {
        await localStorageService.RemoveItemAsync("accessToken");
        await localStorageService.RemoveItemAsync("refreshToken");

        var identity = new ClaimsIdentity();

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }
}
