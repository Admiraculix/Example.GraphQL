using GraphQL.API.Schema;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions(); //can be Redis also

builder.Services.AddCors();

var app = builder.Build();
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