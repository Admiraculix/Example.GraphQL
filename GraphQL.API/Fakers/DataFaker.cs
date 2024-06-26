﻿using Bogus;
using GraphQL.API.Schema.Courses.CourseQueries;
using GraphQL.API.Schema.Enums;
using GraphQL.API.Schema.Instuctors.InstructorQueries;
using GraphQL.API.Schema.Students.StudentQueries;

namespace GraphQL.API.Fakers;

public class DataFaker
{
    private readonly Faker<InstructorType> _instructorFaker;
    private readonly Faker<StudentType> _studentFaker;
    private readonly Faker<CourseType> _courseFaker;

    public DataFaker()
    {
        _instructorFaker = new Faker<InstructorType>()
          .RuleFor(c => c.Id, f => Guid.NewGuid())
          .RuleFor(c => c.FirstName, f => f.Name.FirstName())
          .RuleFor(c => c.LastName, f => f.Name.LastName())
          .RuleFor(c => c.Salary, f => f.Random.Double(0, 100000));

        _studentFaker = new Faker<StudentType>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.GPA, f => f.Random.Double(1, 4));

        _courseFaker = new Faker<CourseType>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.Name, f => f.Name.JobTitle())
            .RuleFor(c => c.Subject, f => f.PickRandom<SubjectType>())
            .RuleFor(c => c.InstructorId, _instructorFaker.Generate().Id)
            .RuleFor(c => c.Students, _ => _studentFaker.Generate(3));
    }

    public IEnumerable<CourseType> GetCourses()
    {
        return _courseFaker.Generate(5);
    }

    public async Task<CourseType> GetCourseByIdAsync(Guid id)
    {
        CourseType course = _courseFaker.Generate();
        course.Id = id;

        return await Task.Run(() => course);
    }
}