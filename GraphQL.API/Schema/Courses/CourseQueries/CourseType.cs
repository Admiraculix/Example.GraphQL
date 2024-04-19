using GraphQL.API.DataLoaders;
using GraphQL.API.Schema.Enums;
using GraphQL.API.Schema.Instuctors.InstructorQueries;
using GraphQL.API.Schema.Students.StudentQueries;
using GraphQL.API.Schema.Users;
using GraphQL.Domain.Entities;
using HotChocolate.Data;

namespace GraphQL.API.Schema.Courses.CourseQueries;

public class CourseType : ISearchResultType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public SubjectType Subject { get; set; }

    [IsProjected(true)]
    public Guid InstructorId { get; set; }

    [GraphQLNonNullType]
    public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader)
    {
        Instructor instructorDTO = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None);

        return new InstructorType()
        {
            Id = instructorDTO.Id,
            FirstName = instructorDTO.FirstName,
            LastName = instructorDTO.LastName,
            Salary = instructorDTO.Salary,
        };
    }

    public IEnumerable<StudentType> Students { get; set; }

    [IsProjected(true)]
    public string CreatorId { get; set; }

    public async Task<UserType> Creator([Service] UserDataLoader userDataLoader)
    {
        if (CreatorId == null)
        {
            return null;
        }

        return await userDataLoader.LoadAsync(CreatorId, CancellationToken.None);
    }
}