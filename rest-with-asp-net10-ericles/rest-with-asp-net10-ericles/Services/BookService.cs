using Mapster;
using rest_with_asp_net10_ericles.Data.DTO;
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

    public BookDTO Create(BookDTO book)
    {
        var entity = book.Adapt<Book>();
        entity = _repositoryBook.Create(entity);
        return entity.Adapt<BookDTO>();
    }

    public bool Delete(long id)
    {
        return _repositoryBook.Delete(id);
    }

    public List<BookDTO> FindAll()
    {
        return _repositoryBook.FindAll().Adapt<List<BookDTO>>();
    }

    public BookDTO FindById(long id)
    {
        return _repositoryBook.FindById(id).Adapt<BookDTO>();
    }

    public BookDTO Update(BookDTO book)
    {
        var entity = book.Adapt<Book>();
        entity = _repositoryBook.Update(entity);
        return entity.Adapt<BookDTO>();
    }
}
