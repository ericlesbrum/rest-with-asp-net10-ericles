using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;

namespace rest_with_asp_net10_ericles.Repositories.Interfaces;

public interface IPersonRepository :IRepository<Person>
{
    Person? Disable(long id);
}
