using Fornecedores.Entities;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornecedores.Services
{
    public class BairroService
    {
        public int Salvar(Bairro bairro,
          [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                ValidarParametros(bairro);

                Bairro exist = unitOfWork.BairroRepository().Get(bairro.Id);
                if (exist.Id >0)
                {
                    bairro.Id = exist.Id;
                    if (exist.Nome.ToUpper() != bairro.Nome.ToUpper()
                        || exist.IdCidade != bairro.IdCidade)
                    {
                        unitOfWork.BairroRepository().Update(bairro);
                    }                    
                }
                else
                {
                    unitOfWork.BairroRepository().Add(bairro);
                    bairro.Id = unitOfWork.BairroRepository().Get(0, bairro.Nome, bairro.IdCidade).Id;
                }

                return bairro.Id;
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
                    throw new ArgumentException("Campo de Identificação Bairro Inválido");

                if (unitOfWork.EnderecoRepository().GetAll().Select(e=> e.IdBairro == id).Count() < 2)
                    unitOfWork.BairroRepository().Remove(id);

                return true;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        public List<Bairro> SelecionarBairros([FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.BairroRepository().GetAll();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        private void ValidarParametros(Bairro bairro)
        {
            if (string.IsNullOrWhiteSpace(bairro.Nome))
                throw new ArgumentException("Campo Nome Bairro é Obrigatório");
            if (bairro.IdCidade < 1)
                throw new ArgumentException("Campo Cidade é Obrigatório");
        }
    }
}
