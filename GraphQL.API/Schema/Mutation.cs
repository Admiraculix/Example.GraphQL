using GraphQL.API.Schema.Courses.CourseMutations;
using HotChocolate.Subscriptions;

namespace GraphQL.API.Schema;

public class Mutation
{
    private readonly List<CourseResult> _courses;

    public Mutation()
    {
        _courses = new List<CourseResult>();
    }

    public async Task<CourseResult> CreateCourseAsync(CourseInputType courseInput, [Service] ITopicEventSender sender)
    {
        CourseResult courseResult = new CourseResult()
        {
            Id = Guid.NewGuid(),
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };

        _courses.Add(courseResult);
        await sender.SendAsync(nameof(Subscription.CourseCreated), courseResult);

        return courseResult;
    }

    public async Task<CourseResult> UpdateCourseAsync(Guid id, CourseInputType courseInput, [Service] ITopicEventSender sender)
    {
        CourseResult? course = _courses.Find(c => c.Id == id)
            ?? throw new GraphQLException(
                new Error("Course not found.", "COURSE_NOT_FOUND"));

        course.Name = courseInput.Name;
        course.Subject = courseInput.Subject;
        course.InstructorId = courseInput.InstructorId;

        string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdatedAsync)}";
        await sender.SendAsync(updateCourseTopic, course);

        return course;
    }

    public bool DeleteCourse(Guid id)
    {
        return _courses.RemoveAll(c => c.Id == id) >= 1;
    }
}