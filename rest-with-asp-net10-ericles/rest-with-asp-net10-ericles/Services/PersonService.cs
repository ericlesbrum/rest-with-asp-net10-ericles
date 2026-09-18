using Mapster;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Hypermedia.Utils;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Repositories.Interfaces;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repositoryPerson;

        public PersonService(IPersonRepository personRepository)
        {
            _repositoryPerson = personRepository;
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

        public PersonDTO? Disable(long id)
        {
            var person = _repositoryPerson.Disable(id);
            return person?.Adapt<PersonDTO>();
        }

        public List<PersonDTO> FindByName(string firstName, string lastName)
        {
            return _repositoryPerson.FindByName(firstName, lastName).Adapt<List<PersonDTO>>();
        }

        public PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var (query, countQuery, sort, size, offset) = BuildQueries(name, sortDirection, pageSize, page);
            var persons = _repositoryPerson.FindWithPagedSearch(query);
            var totalResults = _repositoryPerson.GetCount(countQuery);
            return new PagedSearchDTO<PersonDTO>
            {
                CurrentPage = page,
                List = persons.Adapt<List<PersonDTO>>(),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        private (string query, string countQuery, string sort, int size, int offset)
            BuildQueries(string name, string sortDirection, int pageSize, int page)
        {
            page = Math.Max(1, page);

            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? 1 : pageSize;
            var sort = (!string.IsNullOrWhiteSpace(sortDirection) &&
                !sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)) ? "asc" : "desc";
            var whereClause = $" FROM dbo.person p WHERE 1=1 ";
            if (!string.IsNullOrWhiteSpace(name))
                whereClause += $" AND (p.first_name LIKE '%{name}%') ";

            var query = $@"
                SELECT * {whereClause}
                ORDER BY p.first_name {sort}
                OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY
            ";
            var countQuery = $"SELECT COUNT(*) {whereClause}";

            return (query, countQuery, sort, size, offset);
        }
    }
}
