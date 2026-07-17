using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Model.Context;
using rest_with_asp_net10_ericles.Repositories.Interfaces;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Person> _repositoryPerson;

        public PersonService(IRepository<Person> repositoryPerson)
        {
            _repositoryPerson = repositoryPerson;
        }

        public Person Create(Person person)
        {
            return _repositoryPerson.Create(person);
        }
        public bool Delete(long id)
        {
            return _repositoryPerson.Delete(id);
        }

        public List<Person> FindAll()
        {
            return _repositoryPerson.FindAll();
        }

        public Person FindById(long id)
        {
            return _repositoryPerson.FindById(id);
        }

        public Person Update(Person person)
        {
            return _repositoryPerson.Update(person);
        }
    }
}
