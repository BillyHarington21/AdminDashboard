// Endpoints/MapDashboardEndpoints.cs
using Microsoft.EntityFrameworkCore;
using AdminDashboard.Api.Data;
using AdminDashboard.Api.Models;

namespace AdminDashboard.Endpoints;

public static class MapDashboardEndpoints
{
    public static void MapDashboard(this WebApplication app)
    {
        var group = app.MapGroup("/")
            .RequireAuthorization();

            group.MapGet("/payments", async (int take, AppDbContext db) =>
        {
            var payments = await db.Payments
                .OrderByDescending(p => p.Date)
                .Take(take)
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    ClientId = p.ClientId,
                    Amount = p.Amount,
                    Date = p.Date
                })
                .ToListAsync();

            return Results.Ok(payments);
        });

        group.MapGet("/rate", async (AppDbContext db) =>
        {
            var rate = await db.TokenRates.FirstOrDefaultAsync();
            return rate is null
                ? Results.NotFound()
                : Results.Ok(new { rate.Value });
        });

        group.MapPost("/rate", async (RateDto dto, AppDbContext db) =>
        {
            var rate = await db.TokenRates.FirstOrDefaultAsync();
            if (rate == null)
            {
                rate = new TokenRate { Value = dto.Value };
                db.TokenRates.Add(rate);
            }
            else
            {
                rate.Value = dto.Value;
            }

            await db.SaveChangesAsync();
            return Results.Ok(new { rate.Value });
        });
    }
}
