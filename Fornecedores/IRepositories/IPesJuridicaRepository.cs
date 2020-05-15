using Fornecedores.Entities;
using Fornecedores.Entities.ViewEntities;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface IPesJuridicaRepository
    {
        void Add(PesJuridica obj);
        void Remove(int id);
        void Update(PesJuridica obj);
        PesJuridicaView Get(int id, string cnpj);
        List<PesJuridicaView> GetAll();
    }
}
