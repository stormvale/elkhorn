var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapOpenApi();

app.MapGet("/", () => "Hello World!").WithSummary("Hello World");

await app.RunAsync();