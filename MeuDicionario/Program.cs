using MeuDicionario.Infra;
using MeuDicionario.Model.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDictionaryContex>();
builder.Services.AddTransient<WordDAL>();

builder.Services.AddAutoMapper(typeof(DomainToDTOMapping));

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
