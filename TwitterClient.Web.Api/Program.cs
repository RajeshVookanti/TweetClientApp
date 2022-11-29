using Microsoft.Extensions.Options;
using TwitterClient.Application;
using TwitterClient.DataAccess;
using TwitterClient.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddCors(options => 
{
    {
        options.AddPolicy("allowlocalhost",
            builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(hostname => true)
            );
    } 
});
builder.Services.Configure<AppSettings>(builder.Configuration);
// Add services to the container.
builder.Services.AddDBServices(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTweetAnalysisServices();

var app = builder.Build();
app.UseCors("allowlocalhost");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.SubscribeToTwitterStream();

app.Run();
