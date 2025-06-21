using AdminDashboard.Api.Models;

namespace AdminDashboard.Api.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Clients.Any())
        {
            var clients = new List<Client>
            {
                new() { Name = "Alice", Email = "alice@example.com", BalanceT = 100 },
                new() { Name = "Bob", Email = "bob@example.com", BalanceT = 250 },
                new() { Name = "Charlie", Email = "charlie@example.com", BalanceT = 75 }
            };

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }

        if (!context.Payments.Any())
        {
            var payments = new List<Payment>
            {
                new() { Date = DateTime.UtcNow.AddDays(-1), Amount = 20, ClientId = 1 },
                new() { Date = DateTime.UtcNow.AddDays(-2), Amount = 15, ClientId = 1 },
                new() { Date = DateTime.UtcNow.AddDays(-1), Amount = 50, ClientId = 2 },
                new() { Date = DateTime.UtcNow.AddDays(-3), Amount = 30, ClientId = 2 },
                new() { Date = DateTime.UtcNow, Amount = 10, ClientId = 3 },
            };

            context.Payments.AddRange(payments);
            context.SaveChanges();
        }

        if (!context.TokenRates.Any())
        {
            context.TokenRates.Add(new TokenRate
            {
                Value = 10,
                UpdatedAt = DateTime.UtcNow
            });
            context.SaveChanges();
        }
    }
}
