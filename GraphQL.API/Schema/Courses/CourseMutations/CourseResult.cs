using GraphQL.API.Schema.Enums;

namespace GraphQL.API.Schema.Courses.CourseMutations;

public class CourseResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Subject Subject { get; set; }
    public Guid InstructorId { get; set; }
}