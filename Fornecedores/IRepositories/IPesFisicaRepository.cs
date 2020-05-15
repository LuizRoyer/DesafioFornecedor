using Fornecedores.Entities;
using Fornecedores.Entities.ViewEntities;
using System;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface IPesFisicaRepository
    {
        void Add(PesFisica obj);
        void Remove(int id);
        void Update(PesFisica obj);
        PesFisicaView Get(int id, string cpf);
        DateTime GetDataNascimento(int idPessoa);
        List<PesFisicaView> GetAll();

    }
}
