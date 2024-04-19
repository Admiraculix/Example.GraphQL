using AppAny.HotChocolate.FluentValidation;
using GraphQL.API.Schema.Instuctors.InstructorMutations;
using GraphQL.API.Validators;
using GraphQL.Domain.Entities;
using GraphQL.Persistence.MSSql;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Subscriptions;

namespace GraphQL.API.Schema;

[ExtendObjectType(typeof(Mutation))]
public class InstructorMutation
{
    [Authorize]
    public async Task<InstructorResult> CreateInstructor(
        [UseFluentValidation, UseValidator<InstructorTypeInputValidator>] InstructorTypeInput instructorInput,
        SchoolDbContext context,
        [Service] ITopicEventSender topicEventSender)
    {
        Instructor instructor = new Instructor()
        {
            FirstName = instructorInput.FirstName,
            LastName = instructorInput.LastName,
            Salary = instructorInput.Salary,
        };

        context.Add(instructor);
        await context.SaveChangesAsync();

        InstructorResult instructorResult = new InstructorResult()
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName,
            LastName = instructor.LastName,
            Salary = instructor.Salary,
        };

        await topicEventSender.SendAsync(nameof(Subscription.InstructorCreated), instructorResult);

        return instructorResult;
    }

    [Authorize]
    public async Task<InstructorResult> UpdateInstructor(
        Guid id,
        [UseFluentValidation, UseValidator<InstructorTypeInputValidator>] InstructorTypeInput instructorInput,
        SchoolDbContext context)
    {
        Instructor instructor = await context.Instructors.FindAsync(id);

        if (instructor == null)
        {
            throw new GraphQLException(new Error("Instructor not found.", "INSTRUCTOR_NOT_FOUND"));
        }

        instructor.FirstName = instructorInput.FirstName;
        instructor.LastName = instructorInput.LastName;
        instructor.Salary = instructorInput.Salary;

        context.Update(instructor);
        await context.SaveChangesAsync();

        InstructorResult instructorResult = new InstructorResult()
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName,
            LastName = instructor.LastName,
            Salary = instructor.Salary,
        };

        return instructorResult;
    }

    [Authorize(Policy = "IsAdmin")]
    public async Task<bool> DeleteInstructor(Guid id, SchoolDbContext context)
    {
        Instructor instructorDTO = new Instructor()
        {
            Id = id
        };

        context.Remove(instructorDTO);

        try
        {
            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}