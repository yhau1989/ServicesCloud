using Core.Interfaces;
using Infraestructure;
using Infraestructure.Compras;
using Infraestructure.PostSalesServices;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Injeccion de dependencias
builder.Services.AddTransient<IHistoricoCompras, HistoricoCompras>();
builder.Services.AddTransient<IPostSalesServices, PostSalesServices>();



builder.Services.AddScoped<ValidationFilter>();

// para menejar las exepciones globales
builder.Services.AddControllers(options => {
    options.Filters.Add<GlobalException>();
    options.Filters.Add<ValidationFilter>();

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
