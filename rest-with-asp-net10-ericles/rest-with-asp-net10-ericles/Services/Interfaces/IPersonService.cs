using rest_with_asp_net10_ericles.Data.DTO.V2;

namespace rest_with_asp_net10_ericles.Services.Interfaces;

public interface IPersonService
{
    PersonDTO Create(PersonDTO person);
    PersonDTO FindById(long id);
    List<PersonDTO> FindAll();
    PersonDTO Update(PersonDTO person);
    bool Delete(long id);
}
