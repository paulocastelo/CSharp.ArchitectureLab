using AuthBasics.Sample.Application.Contracts;
using AuthBasics.Sample.Domain;

namespace AuthBasics.Sample.Application;

public sealed class AuthService(IUserRepository userRepository, JwtService jwtService)
{
    public async Task<UserProfileResponse> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existing = await userRepository.FindByEmailAsync(email);
        if (existing is not null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new AppUser
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        var saved = await userRepository.AddAsync(user);
        return new UserProfileResponse(saved.Id, saved.Email, saved.FullName);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await userRepository.FindByEmailAsync(email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = jwtService.GenerateToken(user);
        return new AuthResponse(token, DateTime.UtcNow.AddHours(1).ToString("O"), new UserProfileResponse(user.Id, user.Email, user.FullName));
    }
}
