using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Model.Context;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Services
{
    public class PersonService : IPersonService
    {
        private readonly MSSQLContext _context;

        public PersonService(MSSQLContext context)
        {
            _context = context;
        }

        public Person Create(Person person)
        {
            var personExist = _context.Persons.FirstOrDefault(p => p.Id == person.Id);
            if(personExist != null)
                return personExist;
            _context.Persons.Add(person);
            _context.SaveChanges();
            return person;
        }

        public void Delete(long id)
        {
            var person = _context.Persons.FirstOrDefault(p => p.Id == id);
            if(person != null)
            {
                _context.Persons.Remove(person);
                _context.SaveChanges();
            }
        }

        public List<Person> FindAll()
        {
            return _context.Persons.ToList();
        }

        public Person FindById(long id)
        {
            return _context.Persons.FirstOrDefault(p => p.Id == id)!;
        }

        public Person Update(Person person)
        {
            var personExist = _context.Persons.FirstOrDefault(p => p.Id == person.Id);
            if(personExist == null)
                throw new InvalidOperationException($"Person cannot finded");

            _context.Entry(personExist).CurrentValues.SetValues(person);
            _context.SaveChanges();
            return personExist;
        }
    }
}
