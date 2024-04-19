using AppAny.HotChocolate.FluentValidation;
using GraphQL.API.Middlewares.UseUser;
using GraphQL.API.Schema.Courses.CourseMutations;
using GraphQL.API.Schema.Enums;
using GraphQL.API.Validators;
using GraphQL.Domain.Entities;
using GraphQL.Persistence.MSSql.Repositories;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace GraphQL.API.Schema;

[ExtendObjectType(typeof(Mutation))]
public class CourseMutation
{
    private readonly CoursesRepository _coursesRepository;

    public CourseMutation(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    [Authorize]
    [UseUser]
    public async Task<CourseResult> CreateCourse(
        [UseFluentValidation, UseValidator<CourseTypeInputValidator>] CourseTypeInput courseInput,
        [Service] ITopicEventSender topicEventSender,
        [User] User user)
    {
        string userId = user.Id;

        Course course = new Course()
        {
            Name = courseInput.Name,
            Subject = (Subject)courseInput.Subject,
            InstructorId = courseInput.InstructorId,
            CreatorId = userId
        };
        course = await _coursesRepository.Create(course);
        course = await _coursesRepository.Create(course);

        CourseResult courseResult = new CourseResult()
        {
            Id = course.Id,
            Name = course.Name,
            Subject = (SubjectType)course.Subject,
            InstructorId = course.InstructorId
        };

        await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

        return courseResult;
    }

    [Authorize]
    [UseUser]
    public async Task<CourseResult> UpdateCourse(Guid id,
        [UseFluentValidation, UseValidator<CourseTypeInputValidator>] CourseTypeInput courseInput,
        [Service] ITopicEventSender topicEventSender,
        [User] User user)
    {
        string userId = user.Id;

        Course course = await _coursesRepository.GetById(id);

        if (course == null)
        {
            throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
        }

        if (course.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission to update this course.", "INVALID_PERMISSION"));
        }

        course.Name = courseInput.Name;
        course.Subject = (Subject)courseInput.Subject;
        course.InstructorId = courseInput.InstructorId;

        course = await _coursesRepository.Update(course);

        CourseResult courseResult = new CourseResult()
        {
            Id = course.Id,
            Name = course.Name,
            Subject = (SubjectType)course.Subject,
            InstructorId = course.InstructorId
        };

        string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
        await topicEventSender.SendAsync(updateCourseTopic, course);

        return courseResult;
    }

    [Authorize(Policy = "IsAdmin")]
    public async Task<bool> DeleteCourse(Guid id)
    {
        try
        {
            return await _coursesRepository.Delete(id);
        }
        catch (Exception)
        {
            return false;
        }
    }
}