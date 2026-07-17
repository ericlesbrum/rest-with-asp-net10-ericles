using rest_with_asp_net10_ericles.Model.Base;

namespace rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;

public interface IRepository<T> where T : BaseEntity
{
    List<T> FindAll();
    T FindById(long id);
    T Create(T item);
    T Update(T item);
    bool Delete(long id);
    bool Exists(long id);
}
