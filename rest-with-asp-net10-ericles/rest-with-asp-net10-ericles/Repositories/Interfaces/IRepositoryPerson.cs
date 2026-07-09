using rest_with_asp_net10_ericles.Model;

namespace rest_with_asp_net10_ericles.Repositories.Interfaces;

public interface IRepositoryPerson
{
    Person Create(Person person);
    Person FindById(long id);
    List<Person> FindAll();
    Person Update(Person person);
    bool Delete(long id);
}
