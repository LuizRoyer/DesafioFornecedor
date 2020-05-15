using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface IEnderecoRepository
    {
        void Add(Endereco obj);
        void Remove(int id);
        void Update(Endereco obj);   
        List<EnderecoView> GetAll();
        EnderecoView Get(int id);
    }
}
