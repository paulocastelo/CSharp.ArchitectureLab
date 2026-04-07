namespace AuthBasics.Sample.Application.Contracts;

public sealed record AuthResponse(string AccessToken, string ExpiresAtUtc, UserProfileResponse User);
