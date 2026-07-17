using Microsoft.EntityFrameworkCore;
using rest_with_asp_net10_ericles.Model.Base;
using rest_with_asp_net10_ericles.Model.Context;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;

namespace rest_with_asp_net10_ericles.Repositories.Generics;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly MSSQLContext _context;
    private readonly DbSet<T> dataset;

    public GenericRepository(MSSQLContext context)
    {
        _context = context;
        dataset = _context.Set<T>();
    }

    public T Create(T item)
    {
        _context.Add(item);
        _context.SaveChanges();
        return item;
    }

    public bool Delete(long id)
    {
        var existingItem = dataset.FirstOrDefault(item => item.Id == id);
        if (existingItem == null)
            return false;
        _context.Remove(existingItem);
        _context.SaveChanges();
        return true;
    }

    public bool Exists(long id)
    {
        return dataset.Any(item => item.Id == id);
    }

    public List<T> FindAll()
    {
        return dataset.ToList();
    }

    public T FindById(long id)
    {
        return dataset.FirstOrDefault(item=>item.Id == id)!;
    }

    public T Update(T item)
    {
        var existingItem = dataset.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem == null)
            throw new KeyNotFoundException($"Item with id {item.Id} not found.");
        _context.Entry(existingItem).CurrentValues.SetValues(item);
        _context.SaveChanges();
        return item;
    }
}
