using Fornecedores.Entities;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface IContatoRepository
    {
        void Add(Contato obj);
        void Remove(int id);
        void Update(Contato obj);
        List<Contato> GetAll(int idPessoa);
        Contato Get(int id,string telefone , string celular , int idPessoa);
        Contato Get(int id);
    }
}
