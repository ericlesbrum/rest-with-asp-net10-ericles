using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;

namespace rest_with_asp_net10_ericles.Repositories.Interfaces;

public interface IPersonRepository :IRepository<Person>
{
    Person? Disable(long id);
    List<Person> FindByName(string firstName, string lastName);
    PagedSearch<Person> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
}
