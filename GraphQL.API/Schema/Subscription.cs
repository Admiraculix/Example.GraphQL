using GraphQL.API.Schema.Courses.CourseMutations;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace GraphQL.API.Schema;

public class Subscription
{
    [Subscribe]
    public CourseResult CourseCreated([EventMessage] CourseResult course) => course;

    [Subscribe(With = nameof(CourseUpdatedAsync))]
    public CourseResult CourseUpdated([EventMessage] CourseResult course) => course;

    public ValueTask<ISourceStream<CourseResult>> CourseUpdatedAsync(Guid courseId, [Service] ITopicEventReceiver receiver)
    {
        string topicName = $"{courseId}_{nameof(CourseUpdatedAsync)}";

        return receiver.SubscribeAsync<CourseResult>(topicName);
    }
}