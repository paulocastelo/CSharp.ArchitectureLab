using Microsoft.EntityFrameworkCore;

namespace OutboxPattern.Sample.Infrastructure;

public sealed class OutboxPublisher(IServiceScopeFactory scopeFactory, ILogger<OutboxPublisher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var pending = await dbContext.OutboxMessages
                .Where(message => message.ProcessedAtUtc == null)
                .OrderBy(message => message.CreatedAtUtc)
                .Take(10)
                .ToListAsync(stoppingToken);

            foreach (var message in pending)
            {
                logger.LogInformation("Publishing {EventType}: {Payload}", message.EventType, message.Payload);
                message.ProcessedAtUtc = DateTime.UtcNow;
            }

            if (pending.Count > 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
