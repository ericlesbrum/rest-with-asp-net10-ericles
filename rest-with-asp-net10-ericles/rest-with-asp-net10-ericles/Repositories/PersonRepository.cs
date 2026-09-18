using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Model.Context;
using rest_with_asp_net10_ericles.Repositories.Generics;
using rest_with_asp_net10_ericles.Repositories.Interfaces;
using rest_with_asp_net10_ericles.Repositories.QueryBuilders;

namespace rest_with_asp_net10_ericles.Repositories;

public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    public PersonRepository(MSSQLContext context) : base(context)
    {
    }

    public Person? Disable(long id)
    {
        var person = _context.Persons.FirstOrDefault(p => p.Id == id);
        if (person == null)
            return null;
        person.Enabled = false;
        _context.SaveChanges();
        return person;
    }

    public List<Person> FindByName(string firstName, string lastName)
    {
        var query = _context.Persons.AsQueryable();
        if(!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(p => p.FirstName.Contains(firstName));

        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(p => p.LastName.Contains(lastName));

        return query.ToList();
    }

    public PagedSearch<Person> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
    {
        var queryBuilder = new PersonQueryBuilder();
        var (query, countQuery, sort, size, offset) = queryBuilder.BuildQueries(name, sortDirection, pageSize, page);
        var persons = base.FindWithPagedSearch(query);
        var totalResults = base.GetCount(countQuery);
        return new PagedSearch<Person>
        {
            CurrentPage = page,
            List = persons,
            PageSize = size,
            SortDirections = sort,
            TotalResults = totalResults
        };
    }
}
