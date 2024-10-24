﻿namespace Overseer.Api.Features.Abstractions;

public class Token
{
    public string AccessToken { get; set; } = string.Empty;

    public int ExpiresIn { get; init; }

    public string RefreshToken { get; set; } = string.Empty;

    public int RefreshExpiresIn { get; init; }
}
