using Fornecedores.Entities;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Fornecedores.Services
{
    public class PessoaService
    {
        public int Salvar(Pessoa pessoa,
           [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {               
                Pessoa exist = unitOfWork.PessoaRepository().Get(pessoa.Id,string.Empty);
                if (exist.Id > 0)
                {
                    pessoa.Id = exist.Id;
                    if (exist.Nome.ToUpper() != pessoa.Nome.ToUpper()
                        || exist.Sobrenome.ToUpper() != pessoa.Sobrenome.ToUpper())
                        unitOfWork.PessoaRepository().Update(pessoa);
                }
                else
                {
                    unitOfWork.PessoaRepository().Add(pessoa);
                    pessoa.Id = unitOfWork.PessoaRepository().Get(0,pessoa.Nome).Id;
                }
                return pessoa.Id;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }

        public bool Deletar(int id,
         [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                if (id < 1)
                    throw new ArgumentException("Campo de Identificação Pessoa Inválido");

                unitOfWork.PessoaRepository().Remove(id);
              
                return true;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }       
    }
}
