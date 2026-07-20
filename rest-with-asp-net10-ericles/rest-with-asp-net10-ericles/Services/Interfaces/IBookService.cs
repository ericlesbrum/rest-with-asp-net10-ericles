using rest_with_asp_net10_ericles.Data.DTO.V1;

namespace rest_with_asp_net10_ericles.Services.Interfaces;

public interface IBookService
{
    BookDTO Create(BookDTO book);
    BookDTO FindById(long id);
    List<BookDTO> FindAll();
    BookDTO Update(BookDTO book);
    bool Delete(long id);
}
