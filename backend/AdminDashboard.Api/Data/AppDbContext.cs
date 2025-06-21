using Microsoft.EntityFrameworkCore;
using AdminDashboard.Api.Models;

namespace AdminDashboard.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<TokenRate> TokenRates => Set<TokenRate>();
}
