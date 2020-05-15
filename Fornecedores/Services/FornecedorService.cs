using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Fornecedores.Services
{
    public class FornecedorService
    {
        public IActionResult Salvar(Fornecedor fornecedor,
           [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                ValidarParametros(fornecedor);
                ValidarEmpresa(fornecedor.IdPessoa, fornecedor.IdEmpresa, unitOfWork);
                fornecedor.DataCadastro = DateTime.Now;

                Fornecedor exist = unitOfWork.FornecedorRepository().Get(fornecedor.Id);
                if (exist.Id > 0)
                {
                    fornecedor.Id = exist.Id;
                    if (exist.IdPessoa != fornecedor.IdPessoa
                        || exist.IdEmpresa != fornecedor.IdEmpresa)
                    {
                        unitOfWork.FornecedorRepository().Update(fornecedor);
                    }
                }
                else
                {
                    unitOfWork.FornecedorRepository().Add(fornecedor);
                    fornecedor.Id = unitOfWork.FornecedorRepository().Get(0, fornecedor.IdPessoa, fornecedor.IdEmpresa).Id;
                }

                unitOfWork.Commit();
                return new OkObjectResult("Cadastrado Com Sucesso");
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }

        public IActionResult Deletar(int id,
         [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                if (id < 1)
                    throw new ArgumentException("Campo de Identificação Fornecedor Inválida");

                unitOfWork.FornecedorRepository().Remove(id);
                unitOfWork.Commit();
                return new OkResult(); ;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }
        public List<FornecedorView> SelecionarFornecedores(string nome, string CPF, string CNPJ, DateTime dataCadastro,
            [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.FornecedorRepository().GetAll(nome, CPF, CNPJ, dataCadastro);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        #region Métodos auxiliares
        private void ValidarParametros(Fornecedor fornecedor)
        {
            if (fornecedor.IdEmpresa < 0)
                throw new ArgumentException("Campo Identificador da Empresa é Obrigatorio");

            if (fornecedor.IdPessoa < 0)
                throw new ArgumentException("Campo Identificador de Pessoa é Obrigatorio");

        }
        /// <summary>
        /// Metodo criado para validar se empresa do Paraná, não permitir cadastrar um fornecedor pessoa física menor de idade
        /// </summary>
        /// <param name="unitOfWork"></param>
        private void ValidarEmpresa(int idPessoa, int idEmpresa, IUnitOfWork unitOfWork)
        {

            if (unitOfWork.PessoaRepository().Get(idPessoa, string.Empty).Id <1)
                throw new Exception("Identificador de Pessoa não Encontrado");

            if (unitOfWork.EmpresaRepository().Get(idEmpresa).IdEmpresa < 1)
                throw new Exception("Identificador da Empresa não Encontrado");

            string uf = unitOfWork.EmpresaRepository().GetEstadoEmpresa(idEmpresa);

            if (!string.IsNullOrWhiteSpace(uf) && uf.Trim() == "PR")
            {
                DateTime dataNasc = unitOfWork.PesFisicaRepository().GetDataNascimento(idPessoa);
                if (dataNasc != new DateTime(9999, 01, 01))
                {
                    TimeSpan ts = DateTime.Now - dataNasc;

                    if ((ts.TotalDays/365) < 18)
                        throw new Exception(" Pessoa precisa Ser maior de Idade no Estado do Paraná");
                }
            }
        }
        #endregion
    }
}
