using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _repositoryBook;

    public BookService(IRepository<Book> repositoryBook)
    {
        _repositoryBook = repositoryBook;
    }

    public Book Create(Book book)
    {
        return _repositoryBook.Create(book);
    }

    public bool Delete(long id)
    {
        return _repositoryBook.Delete(id);
    }

    public List<Book> FindAll()
    {
        return _repositoryBook.FindAll();
    }

    public Book FindById(long id)
    {
        return _repositoryBook.FindById(id);
    }

    public Book Update(Book book)
    {
        return _repositoryBook.Update(book);
    }
}
