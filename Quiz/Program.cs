using Microsoft.EntityFrameworkCore;
using Quiz;
using Quiz.Models;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// ��������� ������
app.MapGet("/", (ApplicationContext db) => db.Users.ToList());
app.MapGet("/api/games", (ApplicationContext db) => db.Games.ToList());

app.MapGet("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    // �������� ������������ �� id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "User not found by id" + id });
    var games = db.Games.Where(p => p.UserId == id);
    int x = 0;
    foreach (var game in games)
    {
        x += game.Points; 
    }
    user.Score = x;
    
    await db.SaveChangesAsync();
    // ���� ������������ ������, ���������� ���
    return Results.Json(user);
});
app.MapGet("/api/games/{id:int}", async (int id, ApplicationContext db) =>
{
    Game? game = await db.Games.FirstOrDefaultAsync(u => u.UserId == id);
    if (game == null) return Results.NotFound(new { message = "Game not found" });
    return Results.Json(game);
});

app.MapDelete("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    User? user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
    if (user == null) return Results.NotFound(new { message = "User Not Found" });
    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});
app.MapDelete("/api/games/{id:int}", async (int id, ApplicationContext db) =>
{
    Game? game = await db.Games.FirstOrDefaultAsync(u => u.UserId == id);
    if (game == null) return Results.NotFound(new { message = "Game not found" });
    db.Games.Remove(game);
    await db.SaveChangesAsync();
    return Results.Json(game);
});
app.MapPost("/api/users", async (User user, ApplicationContext db) =>
{
    // ��������� ������������ � ������
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();

    return db.Users.ToList();
});
app.MapPost("/api/games", async (Game game, ApplicationContext db) =>
{
    await db.Games.AddAsync(game);
    await db.SaveChangesAsync();
    return game;
});

app.MapPut("/api/users", async (User userData, ApplicationContext db) =>
{
    // �������� ������������ �� id
    var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == userData.UserId);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (user == null) return Results.NotFound(new { message = "Game not found" });

    // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������
    user.UserName = userData.UserName;
    user.Password = userData.Password;
    await db.SaveChangesAsync();
    return Results.Json(user);
});
app.MapPut("/api/games", async (Game gameData, ApplicationContext db) =>
{
    var game = await db.Games.FirstOrDefaultAsync(u => u.UserId == gameData.UserId);

    if (game == null) return Results.NotFound(new { message = "Game hot found" });

    game.GameName = gameData.GameName;
    game.Points = gameData.Points;
    await db.SaveChangesAsync();
    return Results.Json(game);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
