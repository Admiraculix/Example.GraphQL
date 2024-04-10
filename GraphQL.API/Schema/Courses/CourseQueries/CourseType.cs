using GraphQL.API.Schema.Enums;
using GraphQL.API.Schema.Instuctors.InstructorQueries;
using GraphQL.API.Schema.Students.StudentQueries;

namespace GraphQL.API.Schema.Courses.CourseQueries;

public class CourseType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Subject Subject { get; set; }

    [GraphQLNonNullType]
    public InstructorType Instructor { get; set; }

    public IEnumerable<StudentType> Students { get; set; }
}