using Fornecedores.Entities;
using Fornecedores.Entities.ViewEntities;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Fornecedores.Services
{
    public class PesFisicaService
    {
        public IActionResult Salvar(PesFisicaView pesFisica,
            [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                ValidarParametros(pesFisica);

                pesFisica.IdPessoa = new PessoaService().Salvar(PopularPessoa(pesFisica), unitOfWork);

                if (pesFisica.IdPessoa < 1)
                    throw new Exception(" Erro ao Cadastrar Pessoa");

                PesFisicaView exist = unitOfWork.PesFisicaRepository().Get(pesFisica.Id, string.Empty);
                if (exist.Id > 0)
                {
                    pesFisica.Id = exist.Id;
                    if (exist.Cpf.Trim() != pesFisica.Cpf.Trim()
                        || exist.DataNascimento != pesFisica.DataNascimento)
                    {
                        unitOfWork.PesFisicaRepository().Update(PopularPesFisica(pesFisica));
                    }
                }
                else
                {
                    unitOfWork.PesFisicaRepository().Add(PopularPesFisica(pesFisica));
                    pesFisica.Id = unitOfWork.PesFisicaRepository().Get(0, pesFisica.Cpf).Id;
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
                    throw new ArgumentException("Campo de Identificação Pessoa Física Inválida");

                PesFisicaView pesFisica = unitOfWork.PesFisicaRepository().Get(id, string.Empty);

                if (pesFisica.IdPessoa < 1)
                    throw new ArgumentException("Campo de Identificação Pessoa Física Inválida");

                if (new PessoaService().Deletar(pesFisica.IdPessoa, unitOfWork))
                    unitOfWork.PesFisicaRepository().Remove(id);
                else
                    throw new ArgumentException("Erro ao Excluir");
                unitOfWork.Commit();
                return new OkResult(); ;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }
        public List<PesFisicaView> SelecionarPesFisica([FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.PesFisicaRepository().GetAll();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        #region Métodos auxiliares
        private void ValidarParametros(PesFisicaView pessoa)
        {
            if (string.IsNullOrWhiteSpace(pessoa.Nome))
                throw new ArgumentException("Campo Nome Pessoa é Obrigatório");

            if (string.IsNullOrWhiteSpace(pessoa.Sobrenome))
                throw new ArgumentException("Campo Sobrenome é Obrigatório");

            if (string.IsNullOrWhiteSpace(pessoa.Cpf))
                throw new ArgumentException("Informe CPF da Pessoa");

            if (pessoa.Cpf.Length != 14)
                throw new ArgumentException("CPF da Pessoa Inválido ");

            if (pessoa.Rg.Length != 12)
                throw new ArgumentException("RG da Pessoa Inválido");

            if (string.IsNullOrWhiteSpace(pessoa.Rg))
                throw new ArgumentException("Campo RG é Obrigatório");

            if (pessoa.DataNascimento == null)
                throw new ArgumentException("Campo Data Nascimento é Obrigatório");
        }
        private PesFisica PopularPesFisica(PesFisicaView pessoa)
        {
            return new PesFisica
            {
                Id = pessoa.Id,
                IdPessoa = pessoa.IdPessoa,
                Rg = pessoa.Rg,
                DataNascimento = pessoa.DataNascimento,
                Cpf = pessoa.Cpf
            };
        }
        private Pessoa PopularPessoa(PesFisicaView pessoa)
        {
            return new Pessoa
            {
                Id = pessoa.IdPessoa,
                Nome = pessoa.Nome,
                Sobrenome = pessoa.Sobrenome,
                Tipo = "F"
            };

        }
        #endregion
    }
}
