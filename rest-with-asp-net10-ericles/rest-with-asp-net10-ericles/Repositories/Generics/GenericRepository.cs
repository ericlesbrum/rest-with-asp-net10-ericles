using Microsoft.EntityFrameworkCore;
using rest_with_asp_net10_ericles.Model.Base;
using rest_with_asp_net10_ericles.Model.Context;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;

namespace rest_with_asp_net10_ericles.Repositories.Generics;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly MSSQLContext _context;
    private readonly DbSet<T> _dataset;

    public GenericRepository(MSSQLContext context)
    {
        _context = context;
        _dataset = _context.Set<T>();
    }

    public T Create(T item)
    {
        _context.Add(item);
        _context.SaveChanges();
        return item;
    }

    public bool Delete(long id)
    {
        var existingItem = _dataset.FirstOrDefault(item => item.Id == id);
        if (existingItem == null)
            return false;
        _context.Remove(existingItem);
        _context.SaveChanges();
        return true;
    }

    public bool Exists(long id)
    {
        return _dataset.Any(item => item.Id == id);
    }

    public List<T> FindAll()
    {
        return _dataset.ToList();
    }

    public T FindById(long id)
    {
        return _dataset.FirstOrDefault(item=>item.Id == id)!;
    }

    public T Update(T item)
    {
        var existingItem = _dataset.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem == null)
            throw new KeyNotFoundException($"Item with id {item.Id} not found.");
        _context.Entry(existingItem).CurrentValues.SetValues(item);
        _context.SaveChanges();
        return item;
    }

    public List<T> FindWithPagedSearch(string query)
    {
        return _dataset.FromSqlRaw(query).ToList();
    }

    public int GetCount(string query)
    {
        using var connection = _context.Database.GetDbConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = query;

        var result = Convert.ToInt32(command.ExecuteScalar());
        return result;
    }
}