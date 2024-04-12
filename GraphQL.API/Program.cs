using GraphQL.API.Schema;
using GraphQL.Persistence.Sqlite;
using GraphQL.Persistence.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions(); //can be Redis also


builder.Services.AddPersistenceSqliteRegistration(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContextFactory = services.GetRequiredService<IDbContextFactory<SchoolDbContext>>();

    using var context = dbContextFactory.CreateDbContext();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Do something;
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();

app.UseWebSockets();

app.MapGraphQL();

app.Run();