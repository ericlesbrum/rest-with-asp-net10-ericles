using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Hypermedia.Utils;

namespace rest_with_asp_net10_ericles.Services.Interfaces;

public interface IPersonService
{
    PersonDTO Create(PersonDTO person);
    PersonDTO FindById(long id);
    List<PersonDTO> FindAll();
    PersonDTO Update(PersonDTO person);
    bool Delete(long id);
    PersonDTO? Disable(long id);
    List<PersonDTO> FindByName(string firstName, string lastName);
    PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
}