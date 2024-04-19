using AppAny.HotChocolate.FluentValidation;
using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Google.Apis.Auth.OAuth2;
using GraphQL.API.DataLoaders;
using GraphQL.API.Schema;
using GraphQL.API.Validators;
using GraphQL.Persistence.MSSql;
using GraphQL.Persistence.MSSql.Extensions;
using GraphQL.Persistence.MSSql.Repositories;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceMSSqlRegistration(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CourseTypeInputValidator>(); // register validators
builder.Services.AddFluentValidationAutoValidation(); // the same old MVC pipeline behavior
builder.Services.AddFluentValidationClientsideAdapters(); // for client side

builder.Services
.AddGraphQLServer()
    .RegisterDbContext<SchoolDbContext>(DbContextKind.Pooled)
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddTypeExtension<CourseQuery>()
    .AddTypeExtension<CourseMutation>()
    .AddTypeExtension<InstructorQuery>()
    .AddTypeExtension<InstructorMutation>()
    .AddInMemorySubscriptions() //It can be use Redis also!
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization()
    .AddFluentValidation(o =>
    {
        o.UseDefaultErrorMapper();
    });

builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(builder.Configuration.GetValue<string>("FIREBASE_CONFIG_PATH"))
}));
builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization(
    o => o.AddPolicy("IsAdmin",
        p => p.RequireClaim(FirebaseUserClaimType.EMAIL, "gbozh.work@gmail.com")));

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();
builder.Services.AddScoped<InstructorDataLoader>();
builder.Services.AddScoped<UserDataLoader>();

builder.Services.AddCors();
var app = builder.Build();

//using var scope = app.Services?.GetService<IServiceScopeFactory>().CreateScope();
//using var context = scope.ServiceProvider
//    .GetRequiredService<IDbContextFactory<SchoolDbContext>>()
//    .CreateDbContext();
//context.Database.Migrate();

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

app.UseAuthentication();
app.UseWebSockets();

app.MapGraphQL();

app.Run();