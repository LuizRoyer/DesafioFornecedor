using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using System;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface IFornecedorRepository
    {
        void Add(Fornecedor obj);
        void Remove(int id);
        void Update(Fornecedor obj);
        Fornecedor Get(int id, int idPessoa, int idEmpresa);
        Fornecedor Get(int id);
        List<FornecedorView> GetAll(string nome, string CPF, string CNPJ ,DateTime dataCadastro);
    }
}
