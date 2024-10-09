using System;

namespace Fejka.Test.GuidDtoTests.Domain;

public class UserGuidDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Address HomeAddress { get; set; }
    public Address WorkAddress { get; set; }
}