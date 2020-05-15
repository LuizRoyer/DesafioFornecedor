using Fornecedores.Entities;
using Fornecedores.Entities.ViewEntities;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Fornecedores.Services
{
    public class PesJuridicaService
    {
        public IActionResult Salvar(PesJuridicaView pesjuridica,
           [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                ValidarParametros(pesjuridica);

                pesjuridica.IdPessoa = new PessoaService().Salvar(PopularPessoa(pesjuridica), unitOfWork);

                if (pesjuridica.IdPessoa < 1)
                    throw new Exception(" Erro ao Cadastrar Pessoa");

                PesJuridicaView exist = unitOfWork.PesJuridicaRepository().Get(pesjuridica.Id, string.Empty);
                if (exist.Id > 0)
                {
                    pesjuridica.Id = exist.Id;
                    if (exist.Cnpj.Trim() != pesjuridica.Cnpj.Trim())
                    {
                        unitOfWork.PesJuridicaRepository().Update(PopularPesJuridica(pesjuridica));
                    }
                }
                else
                {
                    unitOfWork.PesJuridicaRepository().Add(PopularPesJuridica(pesjuridica));
                    pesjuridica.Id = unitOfWork.PesJuridicaRepository().Get(0, pesjuridica.Cnpj).Id;
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
                    throw new ArgumentException("Campo de Identificação Pessoa Juridica Inválida");

                PesJuridicaView pesJuridica = unitOfWork.PesJuridicaRepository().Get(id, string.Empty);

                if (pesJuridica.IdPessoa < 1)
                    throw new ArgumentException("Campo de Identificação Pessoa Juridica Inválida");

                if (new PessoaService().Deletar(pesJuridica.IdPessoa, unitOfWork))
                    unitOfWork.PesJuridicaRepository().Remove(id);
                else
                    throw new ArgumentException("Erro ao Excluir");

                unitOfWork.Commit();
                return new OkResult();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }
        public List<PesJuridicaView> SelecionarPesJuridica([FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.PesJuridicaRepository().GetAll();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        #region Métodos auxiliares
        private Pessoa PopularPessoa(PesJuridicaView pessoa)
        {
            return new Pessoa
            {
                Id = pessoa.IdPessoa,
                Nome = pessoa.Nome,
                Sobrenome = pessoa.Sobrenome,
                Tipo = "J"
            };
        }
        private void ValidarParametros(PesJuridicaView pessoa)
        {
            if (string.IsNullOrWhiteSpace(pessoa.Nome))
                throw new ArgumentException("Campo Nome Pessoa é Obrigatório");
            if (string.IsNullOrWhiteSpace(pessoa.Sobrenome))
                throw new ArgumentException("Campo Sobrenome é Obrigatório");

            if (string.IsNullOrWhiteSpace(pessoa.Cnpj))
                throw new ArgumentException("Informe CNPJ da Pessoa");
        }
        private PesJuridica PopularPesJuridica(PesJuridicaView pessoa)
        {
            return new PesJuridica
            {
                Id = pessoa.Id,
                IdPessoa = pessoa.IdPessoa,
                Cnpj = pessoa.Cnpj
            };
        }
        #endregion
    }
}
