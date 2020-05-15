using Fornecedores.Entities;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornecedores.Services
{
    public class EstadoService
    {
        public int Salvar(Estado estado,
           [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                ValidarParametros(estado);

                Estado exist = unitOfWork.EstadoRepository().Get(estado);
                if (exist.Id >0)
                {
                    estado.Id = exist.Id;
                    if (exist.Nome.ToUpper() != estado.Nome.ToUpper()
                        || exist.Uf.ToUpper() != estado.Uf.ToUpper())
                    {
                        unitOfWork.EstadoRepository().Update(estado);
                    }                   
                }
                else
                {
                    unitOfWork.EstadoRepository().Add(estado);
                    estado.Id = unitOfWork.EstadoRepository().Get(estado).Id;
                }

                return estado.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool Deletar(int id,
          [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                if (id < 1)
                    throw new ArgumentException("Campo de Identificação Inválido");

                if (unitOfWork.EnderecoRepository().GetAll().Where(c => c.IdEstado == id).Count() < 2)
                    unitOfWork.EstadoRepository().Remove(id);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Estado> SelecionarEstados([FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.EstadoRepository().GetAll();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidarParametros(Estado estado)
        {
            if (string.IsNullOrWhiteSpace(estado.Nome))
                throw new ArgumentException("Campo Nome Estado é Obrigatório");
            if (string.IsNullOrWhiteSpace(estado.Uf))
                throw new ArgumentException("Campo UF é Obrigatório");
        }
    }
}
