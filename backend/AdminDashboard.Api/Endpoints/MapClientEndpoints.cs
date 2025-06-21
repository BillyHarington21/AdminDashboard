using AdminDashboard.Api.Data;
using AdminDashboard.Api.Models;
using Microsoft.EntityFrameworkCore;

public static class MapClientEndpoints
{
    public static void MapClientApi(this WebApplication app)
    {
        var group = app.MapGroup("/clients").RequireAuthorization();

       group.MapGet("/", async (AppDbContext db) =>
    {
        var clients = await db.Clients
            .Select(c => new ClientDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                BalanceT = c.BalanceT
            })
            .ToListAsync();

        return Results.Ok(clients);
    });

        group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var client = await db.Clients.FindAsync(id);
            return client is null ? Results.NotFound() : Results.Ok(client);
        });

        group.MapPost("/", async (Client client, AppDbContext db) =>
        {
            db.Clients.Add(client);
            await db.SaveChangesAsync();
            return Results.Created($"/clients/{client.Id}", client);
        });

        group.MapPut("/{id:int}", async (int id, Client updated, AppDbContext db) =>
        {
            var client = await db.Clients.FindAsync(id);
            if (client is null) return Results.NotFound();

            client.Name = updated.Name;
            client.Email = updated.Email;
            client.BalanceT = updated.BalanceT;

            await db.SaveChangesAsync();
            return Results.Ok(client);
        });

        group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var client = await db.Clients.FindAsync(id);
            if (client is null) return Results.NotFound();

            db.Clients.Remove(client);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
