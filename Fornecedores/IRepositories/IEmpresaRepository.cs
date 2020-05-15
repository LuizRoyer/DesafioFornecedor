using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using System.Collections.Generic;

namespace Fornecedores.IRepositories
{
    public interface IEmpresaRepository
    {
        void Add(Empresa obj);
        void Remove(int id);
        void Update(Empresa obj);
        List<EmpresaView> GetAll();
        EmpresaView Get(int id);
        string GetEstadoEmpresa(int id);

    }
}
