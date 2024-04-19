using GraphQL.API.Schema.Courses.CourseQueries;
using GraphQL.API.Schema.Enums;
using GraphQL.API.Schema.Instuctors.InstructorQueries;
using GraphQL.Persistence.MSSql;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.API.Schema;

public class Query
{
    public async Task<IEnumerable<ISearchResultType>> Search(string term, SchoolDbContext context)
    {
        IEnumerable<CourseType> courses = await context.Courses
            .Where(c => c.Name.Contains(term))
            .Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = (SubjectType)c.Subject,
                InstructorId = c.InstructorId,
                CreatorId = c.CreatorId
            })
            .ToListAsync();

        IEnumerable<InstructorType> instructors = await context.Instructors
            .Where(i => i.FirstName.Contains(term) || i.LastName.Contains(term))
            .Select(i => new InstructorType()
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Salary = i.Salary,
            })
            .ToListAsync();

        var res = new List<ISearchResultType>()
            .Concat(courses)
            .Concat(instructors);

        return res;
    }

    [GraphQLDeprecated("This query is deprecated!")]
    public string Instructions => "Example of Query with simple text value!";
}