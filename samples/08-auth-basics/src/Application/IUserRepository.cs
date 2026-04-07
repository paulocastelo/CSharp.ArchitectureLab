using AuthBasics.Sample.Domain;

namespace AuthBasics.Sample.Application;

public interface IUserRepository
{
    Task<AppUser?> FindByEmailAsync(string email);
    Task<AppUser> AddAsync(AppUser user);
}
