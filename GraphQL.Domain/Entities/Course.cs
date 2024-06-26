﻿namespace GraphQL.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Subject Subject { get; set; }
    public string CreatorId { get; set; }

    public Guid InstructorId { get; set; }
    public Instructor Instructor { get; set; }

    public IEnumerable<Student> Students { get; set; }
}