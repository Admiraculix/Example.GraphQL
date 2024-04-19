using GraphQL.API.Schema.Courses.CourseQueries;
using GraphQL.API.Schema.Enums;
using GraphQL.API.Schema.Sorters;
using GraphQL.Domain.Entities;
using GraphQL.Persistence.MSSql;
using HotChocolate.Data;

namespace GraphQL.API.Schema;

[ExtendObjectType(typeof(Query))]
public class CourseQuery
{
    //[UseDbContext(typeof(SchoolDbContext))]
    [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
    [UseProjection]
    [UseFiltering(typeof(CourseFilterType))]
    [UseSorting(typeof(CourseSortType))]
    public IQueryable<CourseType> GetSpecificCourses(SchoolDbContext context)
    {
        return context.Courses.Select(c => new CourseType()
        {
            Id = c.Id,
            Name = c.Name,
            Subject = (SubjectType)c.Subject,
            InstructorId = c.InstructorId,
            CreatorId = c.CreatorId
        });
    }
    public IQueryable<CourseType> GetCourses(SchoolDbContext context)
    {
        return context.Courses.Select(c => new CourseType()
        {
            Id = c.Id,
            Name = c.Name,
            Subject = (SubjectType)c.Subject,
            InstructorId = c.InstructorId,
            CreatorId = c.CreatorId
        });
    }

    public async Task<CourseType> GetCourseByIdAsync(Guid id, SchoolDbContext context)
    {
        Course course = await context.Courses.FindAsync(id);

        if (course == null)
        {
            return null;
        }

        return new CourseType()
        {
            Id = course.Id,
            Name = course.Name,
            Subject = (SubjectType)course.Subject,
            InstructorId = course.InstructorId,
            CreatorId = course.CreatorId
        };
    }
}