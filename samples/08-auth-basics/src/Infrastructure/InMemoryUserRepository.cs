using AuthBasics.Sample.Application;
using AuthBasics.Sample.Domain;

namespace AuthBasics.Sample.Infrastructure;

public sealed class InMemoryUserRepository : IUserRepository
{
    private static readonly List<AppUser> Users = [];

    public Task<AppUser?> FindByEmailAsync(string email) =>
        Task.FromResult(Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));

    public Task<AppUser> AddAsync(AppUser user)
    {
        Users.Add(user);
        return Task.FromResult(user);
    }
}
