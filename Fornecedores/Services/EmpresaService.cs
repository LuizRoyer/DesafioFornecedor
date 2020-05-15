using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Fornecedores.Services
{
    public class EmpresaService
    {
        public IActionResult Salvar(Empresa empresa,
            [FromServices] IUnitOfWork unitOfWork    )
        {
            try
            {
                ValidarParametros(empresa, unitOfWork);
               
                EmpresaView exit = unitOfWork.EmpresaRepository().Get(empresa.Id);
                if ( exit.IdEmpresa > 0)
                {
                    empresa.Id = exit.IdEmpresa;
                    if (!ValidarAlteracao(empresa, exit))
                        unitOfWork.EmpresaRepository().Update(empresa);
                }
                else
                    unitOfWork.EmpresaRepository().Add(empresa);

                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
            return new OkObjectResult("Cadastrado Com Sucesso");
        }
        public IActionResult Deletar(int id,
         [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                if (id < 1)
                    return new BadRequestObjectResult("Campo de Identificação Empresa Inválido");

                EmpresaView empresa = unitOfWork.EmpresaRepository().Get(id);

                if (string.IsNullOrWhiteSpace(empresa.CNPJ))
                    return new BadRequestObjectResult("Campo de Identificação Empresa Inválido");
                              
                new EnderecoService().Deletar(empresa.Endereco.IdEndereco, unitOfWork, true);              
                unitOfWork.EmpresaRepository().Remove(id);

                unitOfWork.Commit();
                return new OkResult();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }
        public List<EmpresaView> SelecionarEmpresas(
                                [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.EmpresaRepository().GetAll();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        #region Métodos auxiliares
        private bool ValidarAlteracao(Empresa empresa, EmpresaView empresaView)
        {            
            if (empresa.NomeFantasia.ToUpper() != empresaView.NomeFantasia.ToUpper())
                return false;
            if (empresa.IdEndereco != empresaView.Endereco.IdEndereco)
                return false;

            return true;
        }

        private void ValidarParametros(Empresa empresa, IUnitOfWork unit)
        {
            if (string.IsNullOrWhiteSpace(empresa.NomeFantasia))
                throw new ArgumentException("Campo Nome Fantasia é Obrigatório ");

            if (empresa.IdEndereco <1)
                throw new ArgumentException("Campo Endereço é Obrigatório ");

            if(unit.EnderecoRepository().Get(empresa.IdEndereco).IdEndereco <1)
                throw new ArgumentException("Endereço Informado não Encontrado ");

            if(empresa.CNPJ.Length != 18)
                throw new ArgumentException("CNPJ Informado é Inálido ");
        }
        #endregion
    }
}
