using Mapster;
using rest_with_asp_net10_ericles.Data.DTO;
using rest_with_asp_net10_ericles.Model;
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

        public PersonDTO Create(PersonDTO person)
        {
            var entity = person.Adapt<Person>();
            entity = _repositoryPerson.Create(entity);
            return entity.Adapt<PersonDTO>();
        }
        public bool Delete(long id)
        {
            return _repositoryPerson.Delete(id);
        }

        public List<PersonDTO> FindAll()
        {
            return _repositoryPerson.FindAll().Adapt<List<PersonDTO>>();
        }

        public PersonDTO FindById(long id)
        {
            return _repositoryPerson.FindById(id).Adapt<PersonDTO>();
        }

        public PersonDTO Update(PersonDTO person)
        {
            var entity = person.Adapt<Person>();
            entity = _repositoryPerson.Update(entity);
            return entity.Adapt<PersonDTO>();
        }
    }
}
