using FluentAssertions;
using Moq;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;
using rest_with_asp_net10_ericles.Services;

namespace rest_with_asp_net10_ericles.Tests;

public class PersonServiceTests
{
    [Fact]
    public void Create_ShouldMapPersonDtoToEntityAndReturnDto()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Person>>();
        var person = new PersonDTO
        {
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main Street",
            Gender = "Male",
            Birthday = new DateTime(1990, 1, 1)
        };

        repositoryMock
            .Setup(r => r.Create(It.IsAny<Person>()))
            .Returns((Person person) =>
            {
                person.Id = 7;
                return person;
            });

        var service = new PersonService(repositoryMock.Object);

        // Act
        var result = service.Create(person);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(7);
        result.FirstName.Should().Be(person.FirstName);
        result.LastName.Should().Be(person.LastName);
        result.Address.Should().Be(person.Address);
        result.Gender.Should().Be(person.Gender);
        result.Birthday.Should().Be(person.Birthday);
        repositoryMock.Verify(
            r => r.Create(It.Is<Person>(p =>
                p.FirstName == person.FirstName &&
                p.LastName == person.LastName &&
                p.Address == person.Address &&
                p.Gender == person.Gender &&
                p.Birthday == person.Birthday)),
            Times.Once);
    }

    [Fact]
    public void FindAll_ShouldReturnAllPersonsAsDtos()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Person>>();
        var people = new List<Person>
        {
            new()
            {
                Id = 1,
                FirstName = "Ana",
                LastName = "Silva",
                Address = "Rua A",
                Gender = "Female",
                Birthday = new DateTime(1995, 5, 10)
            },
            new()
            {
                Id = 2,
                FirstName = "Carlos",
                LastName = "Mendes",
                Address = "Rua B",
                Gender = "Male",
                Birthday = new DateTime(1988, 8, 20)
            }
        };

        repositoryMock.Setup(r => r.FindAll()).Returns(people);
        var service = new PersonService(repositoryMock.Object);

        // Act
        var result = service.FindAll();

        // Assert
        result.Should().HaveCount(2);
        result[0].FirstName.Should().Be("Ana");
        result[0].LastName.Should().Be("Silva");
        result[1].FirstName.Should().Be("Carlos");
        result[1].LastName.Should().Be("Mendes");

        repositoryMock.Verify(r => r.FindAll(), Times.Once);
    }

    [Fact]
    public void FindById_ShouldReturnPersonDto_WhenPersonExists()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Person>>();
        var person = new Person
        {
            Id = 10,
            FirstName = "Maria",
            LastName = "Souza",
            Address = "Rua C",
            Gender = "Female",
            Birthday = new DateTime(1992, 2, 14)
        };

        repositoryMock.Setup(r => r.FindById(10)).Returns(person);
        var service = new PersonService(repositoryMock.Object);

        // Act
        var result = service.FindById(10);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(10);
        result.FirstName.Should().Be("Maria");
        result.LastName.Should().Be("Souza");

        repositoryMock.Verify(r => r.FindById(10), Times.Once);
    }

    [Fact]
    public void Update_ShouldMapDtoAndReturnUpdatedPerson()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Person>>();
        var person = new PersonDTO
        {
            Id = 3,
            FirstName = "João",
            LastName = "Pereira",
            Address = "Rua D",
            Gender = "Male",
            Birthday = new DateTime(1985, 7, 9)
        };

        repositoryMock
            .Setup(r => r.Update(It.IsAny<Person>()))
            .Returns((Person person) => person);

        var service = new PersonService(repositoryMock.Object);

        // Act
        var result = service.Update(person);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(3);
        result.FirstName.Should().Be("João");
        result.LastName.Should().Be("Pereira");

        repositoryMock.Verify(
            r => r.Update(It.Is<Person>(p =>
                p.Id == person.Id &&
                p.FirstName == person.FirstName &&
                p.LastName == person.LastName)),
            Times.Once);
    }

    [Fact]
    public void Delete_ShouldReturnTrue_WhenRepositoryDeletesPerson()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Person>>();
        repositoryMock.Setup(r => r.Delete(4)).Returns(true);
        var service = new PersonService(repositoryMock.Object);

        // Act
        var result = service.Delete(4);

        // Assert
        result.Should().BeTrue();
        repositoryMock.Verify(r => r.Delete(4), Times.Once);
    }
}
