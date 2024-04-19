using GraphQL.API.Schema.Courses.CourseQueries;
using HotChocolate.Data.Sorting;

namespace GraphQL.API.Schema.Sorters;

public class CourseSortType : SortInputType<CourseType>
{
    protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
    {
        descriptor.Ignore(c => c.Id);
        descriptor.Ignore(c => c.InstructorId);

        base.Configure(descriptor);
    }
}