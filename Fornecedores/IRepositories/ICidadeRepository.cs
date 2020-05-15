using Fornecedores.Entities;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface ICidadeRepository
    {
        void Add(Cidade obj);
        void Remove(int id);
        void Update(Cidade obj);
        List<Cidade> GetAll();
        Cidade Get(Cidade obj);
    }
}
