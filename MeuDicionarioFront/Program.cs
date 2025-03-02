using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:7168");

var app = builder.Build();

app.UseStaticFiles();
app.MapGet("/", () => Results.Redirect("MeuDicionarioFront-main/index.html"));

app.UseHttpsRedirection();

Process.Start(new ProcessStartInfo("cmd", $"/c start https://localhost:7168")
{
    CreateNoWindow = true
});

app.Run();
