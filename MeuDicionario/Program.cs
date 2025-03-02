using MeuDicionario.Infra;
using MeuDicionario.Model.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<MyDictionaryContex>();
static void ForInit() {
    var contex = new MyDictionaryContex();
    contex.Database.Migrate();
    var wordSerachForRevision = new SearchWordsForRevision(contex);

    wordSerachForRevision.Execute();
}

ForInit();

builder.Services.AddAutoMapper(typeof(DomainToDTOMapping));


builder.WebHost.UseUrls("https://localhost:7167");
builder.Services.AddCors(options =>
{
    options.AddPolicy("Front", policy =>
    {
        policy.WithOrigins("https://localhost:7168")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.UseCors("Front");

app.Run();
