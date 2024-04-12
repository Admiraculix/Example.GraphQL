using Microsoft.EntityFrameworkCore;

using GraphQL.Domain.Entities;

namespace GraphQL.Persistence.Sqlite;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Student> Students { get; set; }
}