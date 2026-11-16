// Sample 6 — a minimal API over Postgres through EF Core. The platform must find: backend, dotnet, health at /health,
// EF migrations under Migrations/ (history table __EFMigrationsHistory), DATABASE_URL as a database reference.
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connection = Environment.GetEnvironmentVariable("DATABASE_URL") ?? builder.Configuration.GetConnectionString("Inventory");
builder.Services.AddDbContext<InventoryDb>(o => o.UseNpgsql(connection));
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { ok = true }));
app.MapGet("/items", async (InventoryDb db) => await db.Items.OrderBy(i => i.Id).ToListAsync());
app.MapPost("/items", async (InventoryDb db, Item item) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");

public class Item { public int Id { get; set; } public string Name { get; set; } = ""; public int Quantity { get; set; } }
public class InventoryDb : DbContext
{
    public InventoryDb(DbContextOptions<InventoryDb> o) : base(o) { }
    public DbSet<Item> Items => Set<Item>();
}
