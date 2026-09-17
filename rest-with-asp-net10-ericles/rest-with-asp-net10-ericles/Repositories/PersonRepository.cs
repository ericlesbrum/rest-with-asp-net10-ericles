using Microsoft.IdentityModel.Tokens;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Model.Context;
using rest_with_asp_net10_ericles.Repositories.Generics;
using rest_with_asp_net10_ericles.Repositories.Interfaces;

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
}
