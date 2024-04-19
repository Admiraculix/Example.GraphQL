using GraphQL.API.Schema.Instuctors.InstructorQueries;
using GraphQL.Domain.Entities;
using GraphQL.Persistence.MSSql;
using HotChocolate.Data;

namespace GraphQL.API.Schema;

[ExtendObjectType(typeof(Query))]
public class InstructorQuery
{
    // Note: If you use more than one middleware, keep in mind that ORDER MATTERS.
    // The correct order is UsePaging > UseProjection > UseFiltering > UseSorting
    // https://chillicream.com/docs/hotchocolate/v13/fetching-data/projections#firstordefault--singleordefault
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<InstructorType> GetInstructors(SchoolDbContext context)
    {
        return context.Instructors.Select(i => new InstructorType
        {
            Id = i.Id,
            FirstName = i.FirstName,
            LastName = i.LastName,
            Salary = i.Salary,
        });
    }

    public async Task<InstructorType> GetInstructorById(Guid id, SchoolDbContext context)
    {
        Instructor instructor = await context.Instructors.FindAsync(id);

        if (instructor == null)
        {
            return null;
        }

        return new InstructorType
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName,
            LastName = instructor.LastName,
            Salary = instructor.Salary,
        };
    }
}