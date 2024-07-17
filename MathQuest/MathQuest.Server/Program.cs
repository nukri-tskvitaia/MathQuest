using MathQuestWebApi;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

// Add services to the container.
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
startup.Configure(app, builder.Environment);

app.MapFallbackToFile("/index.html");

app.Run();
