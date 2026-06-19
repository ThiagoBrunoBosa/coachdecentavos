using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoachDecentavos.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public Task<User?> GetByOAuthAsync(string provider, string providerUserId, CancellationToken cancellationToken = default)
        => _dbContext.Users
            .Include(x => x.OAuthAccounts)
            .FirstOrDefaultAsync(
                x => x.OAuthAccounts.Any(o => o.Provider == provider && o.ProviderUserId == providerUserId),
                cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AddAsync(user, cancellationToken);

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}