using Fornecedores.Entities;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornecedores.Services
{
    public class CidadeService
    {
        public int Salvar(Cidade cidade,
           [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                ValidarParametros(cidade);

                Cidade exist = unitOfWork.CidadeRepository().Get(cidade);
                if (exist.Id >0)
                {
                    cidade.Id = exist.Id;
                    if (exist.Nome.ToUpper() != cidade.Nome.ToUpper()
                        || exist.IdEstado != cidade.IdEstado)
                    {
                        unitOfWork.CidadeRepository().Update(cidade);
                    }                    
                }
                else
                {
                    unitOfWork.CidadeRepository().Add(cidade);
                    cidade.Id = unitOfWork.CidadeRepository().Get(cidade).Id;
                }

                return cidade.Id;
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
                    throw new ArgumentException("Campo de Identificação Cidade Inválido");

                if (unitOfWork.EnderecoRepository().GetAll().Where(b=>b.IdCidade == id).Count() < 2)
                    unitOfWork.CidadeRepository().Remove(id);

                return true;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        public List<Cidade> SelecionarCidades([FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.CidadeRepository().GetAll();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        private void ValidarParametros(Cidade cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade.Nome))
                throw new ArgumentException("Campo Nome Cidade é Obrigatório");
            if (cidade.IdEstado < 1)
                throw new ArgumentException("Campo Estado é Obrigatório");
        }
    }
}
