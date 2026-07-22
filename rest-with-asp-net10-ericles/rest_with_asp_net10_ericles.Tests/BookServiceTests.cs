using FluentAssertions;
using Moq;
using rest_with_asp_net10_ericles.Data.DTO.V1;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;
using rest_with_asp_net10_ericles.Services;

namespace rest_with_asp_net10_ericles.Tests;

public class BookServiceTests
{
    [Fact]
    public void Create_ShoudMapBookDtoToEntityAndReturnDto()
    {
        //Arrange
        var repositoryMock = new Mock<IRepository<Book>>();
        var book = new BookDTO
        {
            Author = "Ziraldo",
            Title = "Menino Maluquinho",
            LaunchDate = new DateTime(1980, 10, 24),
            Price = 68.98m
        };

        repositoryMock
            .Setup(r => r.Create(It.IsAny<Book>()))
            .Returns((Book book) =>
            {
                book.Id = 1;
                return book;
            });

        var service = new BookService(repositoryMock.Object);

        // Act
        var result = service.Create(book);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Author.Should().Be(book.Author);
        result.Title.Should().Be(book.Title);
        result.LaunchDate.Should().Be(book.LaunchDate);
        result.Price.Should().Be(book.Price);
    }

    [Fact]
    public void FindAll_ShouldReturnAllBooksAsDtos()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Book>>();
        var book = new List<Book>
        {
            new()
            {
                Id = 1,
                Author = "Robert C. Martin",
                Title = "Clean Code",
                LaunchDate = new DateTime(2008, 8, 1),
                Price = 149.90m
            },
            new()
            {
                Id = 2,
                Author = "Martin Fowler",
                Title = "Refactoring",
                LaunchDate = new DateTime(2018, 11, 19),
                Price = 199.90m
            }
        };

        repositoryMock.Setup(r => r.FindAll()).Returns(book);
        var service = new BookService(repositoryMock.Object);

        // Act
        var result = service.FindAll();

        // Assert
        result.Should().HaveCount(2);

        result[0].Author.Should().Be("Robert C. Martin");
        result[0].Title.Should().Be("Clean Code");
        result[0].Price.Should().Be(149.90m);

        result[1].Author.Should().Be("Martin Fowler");
        result[1].Title.Should().Be("Refactoring");
        result[1].Price.Should().Be(199.90m);

        repositoryMock.Verify(r => r.FindAll(), Times.Once);
    }

    [Fact]
    public void FindById_ShouldReturnBookDto_WhenBookExists()
    {
        //Arrange
        var repositoryMock = new Mock<IRepository<Book>>();
        var book = new Book
        {
            Id = 1,
            Author = "Ziraldo",
            Title = "Menino Maluquinho",
            LaunchDate = new DateTime(1980, 10, 24),
            Price = 68.98m
        };

        repositoryMock.Setup(r => r.FindById(1)).Returns(book);
        var service = new BookService(repositoryMock.Object);

        // Act
        var result = service.FindById(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Author.Should().Be("Ziraldo");
        result.Title.Should().Be("Menino Maluquinho");
        result.Price.Should().Be(68.98m);
        result.LaunchDate.Should().Be(new DateTime(1980, 10, 24));

        repositoryMock.Verify(r => r.FindById(1), Times.Once);
    }

    [Fact]
    public void Update_ShouldMapDtoAndReturnUpdatedBook()
    {
        var repositoryMock = new Mock<IRepository<Book>>();
        var book = new BookDTO
        {
            Id = 1,
            Author = "Ziraldo",
            Title = "Menino Maluquinho",
            LaunchDate = new DateTime(1980, 10, 24),
            Price = 68.98m
        };

        repositoryMock
            .Setup(r => r.Update(It.IsAny<Book>()))
            .Returns((Book book) => book);

        var service = new BookService(repositoryMock.Object);

        // Act
        var result = service.Update(book);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Author.Should().Be("Ziraldo");
        result.Title.Should().Be("Menino Maluquinho");

        repositoryMock.Verify(
            r => r.Update(It.Is<Book>(p =>
                p.Id == book.Id &&
                p.Author == book.Author &&
                p.Title == book.Title)),
            Times.Once);
    }

    [Fact]
    public void Delete_ShouldReturnTrue_WhenRepositoryDeletesBook()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Book>>();
        repositoryMock.Setup(r => r.Delete(4)).Returns(true);
        var service = new BookService(repositoryMock.Object);

        // Act
        var result = service.Delete(4);

        // Assert
        result.Should().BeTrue();
        repositoryMock.Verify(r => r.Delete(4), Times.Once);
    }
}