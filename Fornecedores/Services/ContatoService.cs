using Fornecedores.Entities;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Fornecedores.Services
{
    public class ContatoService
    {
        public IActionResult Salvar(Contato contato,
             [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                ValidarParametros(contato, unitOfWork);

                Contato exist = unitOfWork.ContatoRepository().Get(contato.Id);
                if (exist.Id > 0)
                {
                    contato.Id = exist.Id;
                    unitOfWork.ContatoRepository().Update(contato);                  
                }
                else
                {
                    unitOfWork.ContatoRepository().Add(contato);
                    contato.Id = unitOfWork.ContatoRepository().Get(0,contato.Telefone, contato.Celular, contato.IdPessoa).Id;
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
                    throw new ArgumentException("Campo de Identificação Contato Inválido");
                                
                    unitOfWork.ContatoRepository().Remove(id);
                unitOfWork.Commit();
                return new OkResult();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }
        public List<Contato> SelecionarContatos(int idPessoa,[FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.ContatoRepository().GetAll(idPessoa);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        private void ValidarParametros(Contato contato , IUnitOfWork unit)
        {
            if (string.IsNullOrWhiteSpace(contato.Telefone)
                && string.IsNullOrWhiteSpace(contato.Celular))
                throw new ArgumentException("Campo Nome Telefone ou Celular devem ser Informados");

            if (contato.IdPessoa < 0)
                throw new ArgumentException("Campo Identificador Pessoa é Obrigatório ");

            if (unit.PessoaRepository().Get(contato.IdPessoa, string.Empty) == null)
                throw new ArgumentException("Campo Identificador Pessoa não Encontrado ");
        }
    }
}
