using GraphQL.API.Schema.Enums;

namespace GraphQL.API.Schema.Courses.CourseMutations;

public class CourseTypeInput
{
    public string Name { get; set; }
    public SubjectType Subject { get; set; }
    public Guid InstructorId { get; set; }
}