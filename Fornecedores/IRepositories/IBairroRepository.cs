using Fornecedores.Entities;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface IBairroRepository
    {
        void Add(Bairro obj);
        void Remove(int id);
        void Update(Bairro obj);
        List<Bairro> GetAll();
        Bairro Get(int id, string nome, int idCidade);
        Bairro Get(int id);
    }
}
