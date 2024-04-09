var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapGraphQL();

app.Run();