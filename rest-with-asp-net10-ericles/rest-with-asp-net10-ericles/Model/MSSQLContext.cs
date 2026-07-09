using Microsoft.EntityFrameworkCore;
using rest_with_asp_net10_ericles.Model;

namespace rest_with_asp_net10_ericles.Model.Context;

public class MSSQLContext : DbContext
{
  public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options)
  {
  }
  public DbSet<Person> Persons { get; set; }
}
