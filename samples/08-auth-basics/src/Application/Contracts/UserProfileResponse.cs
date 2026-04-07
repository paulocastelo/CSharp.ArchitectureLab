namespace AuthBasics.Sample.Application.Contracts;

public sealed record UserProfileResponse(Guid Id, string Email, string FullName);
