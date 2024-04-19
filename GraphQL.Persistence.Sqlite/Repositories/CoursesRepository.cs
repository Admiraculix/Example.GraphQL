using GraphQL.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Persistence.Sqlite.Repositories;

public class CoursesRepository
{
    private readonly IDbContextFactory<SchoolDbContext> _contextFactory;

    public CoursesRepository(IDbContextFactory<SchoolDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<Course>> GetAll()
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Courses.ToListAsync();
        }
    }

    public async Task<Course> GetById(Guid courseId)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        }
    }

    public async Task<Course> Create(Course course)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            return course;
        }
    }

    public async Task<Course> Update(Course course)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            context.Courses.Update(course);
            await context.SaveChangesAsync();

            return course;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            Course course = new Course()
            {
                Id = id
            };
            context.Courses.Remove(course);

            return await context.SaveChangesAsync() > 0;
        }
    }
}