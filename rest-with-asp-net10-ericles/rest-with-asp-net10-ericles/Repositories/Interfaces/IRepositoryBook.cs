using rest_with_asp_net10_ericles.Model;

namespace rest_with_asp_net10_ericles.Repositories.Interfaces;

public interface IRepositoryBook
{
    Book Create(Book book);
    Book FindById(long id);
    List<Book> FindAll();
    Book Update(Book book);
    bool Delete(long id);
}
