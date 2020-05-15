using Fornecedores.Entities;

namespace Fornecedores.IRepositories
{
    public interface IPessoaRepository {
        void Add(Pessoa obj);
        void Remove(int id);
        void Update(Pessoa obj);
        Pessoa Get(int id, string nome);       
    }
}
