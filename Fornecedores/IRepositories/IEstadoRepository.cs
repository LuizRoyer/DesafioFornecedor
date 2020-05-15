using Fornecedores.Entities;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface IEstadoRepository
    {
        void Add(Estado obj);
        void Remove(int id);
        void Update(Estado obj);
        List<Estado> GetAll();
        Estado Get(Estado obj);
    }
}
