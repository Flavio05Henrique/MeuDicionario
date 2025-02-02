using MeuDicionario.Infra;
using MeuDicionario.Infra.DALs;
using MeuDicionario.Model.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<MyDictionaryContex>();
builder.Services.AddSingleton<WordDAL>();
builder.Services.AddSingleton<RevisionDAL>();
builder.Services.AddSingleton<TextDAL>();
builder.Services.AddSingleton<TextWordDAL>();
static void ForInit() {
    var contex = new MyDictionaryContex();
    contex.Database.Migrate();
    var revisonDAL = new RevisionDAL(contex);
    var revisionLogDAL = new RevisionLogDAL(contex);
    var wordDAL = new WordDAL(contex);
    var wordSerachForRevision = new SearchWordsForRevision(revisonDAL, revisionLogDAL, wordDAL);

    wordSerachForRevision.Execute();
}

ForInit();

builder.Services.AddAutoMapper(typeof(DomainToDTOMapping));



builder.WebHost.UseUrls("https://localhost:7167");
var app = builder.Build();

app.UseStaticFiles();
app.MapGet("/", () => Results.Redirect("/MeuDicionarioFront-main/index.html"));


//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
