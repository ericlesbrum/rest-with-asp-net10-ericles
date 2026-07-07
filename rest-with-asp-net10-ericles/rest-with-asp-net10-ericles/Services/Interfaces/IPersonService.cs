using rest_with_asp_net10_ericles.Model;

namespace rest_with_asp_net10_ericles.Services.Interfaces;

public interface IPersonService
{
    Person Create(Person person);
    Person FindById(long id);
    List<Person> FindAll();
    Person Update(Person person);
    void Delete(long id);
}
