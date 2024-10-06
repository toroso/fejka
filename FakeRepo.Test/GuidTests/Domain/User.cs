using System;

namespace FakeRepo.Test.GuidTests.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Address HomeAddress { get; set; }
    public Address WorkAddress { get; set; }
}