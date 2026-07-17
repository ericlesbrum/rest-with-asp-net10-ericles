using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Model.Context;
using rest_with_asp_net10_ericles.Repositories.Interfaces;

namespace rest_with_asp_net10_ericles.Repositories;

public class RepositoryBook : IRepositoryBook
{
    private readonly MSSQLContext _context;

    public RepositoryBook(MSSQLContext context)
    {
        _context = context;
    }

    public Book Create(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
        return book;
    }

    public bool Delete(long id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            return false;
        _context.Books.Remove(book);
        _context.SaveChanges();
        return true;
    }

    public List<Book> FindAll()
    {
        return _context.Books.ToList();
    }

    public Book FindById(long id)
    {
        return _context.Books.FirstOrDefault(b => b.Id == id)!;
    }

    public Book Update(Book book)
    {
        var bookExist = _context.Books.FirstOrDefault(b => b.Id == book.Id);
        if (bookExist == null)
            throw new InvalidOperationException($"Book cannot finded");
        _context.Books.Entry(bookExist).CurrentValues.SetValues(book);
        _context.SaveChanges();
        return bookExist;
    }
}
