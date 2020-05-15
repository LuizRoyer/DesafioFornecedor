using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Fornecedores.Services
{
    public class EnderecoService
    {
        public IActionResult Salvar(EnderecoView enderecoView,
            [FromServices] IUnitOfWork unitOfWork
            )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(enderecoView.Cep))
                    return new BadRequestObjectResult("Campo de Cep do Endereço Inválido");

                enderecoView.IdEstado = new EstadoService().Salvar(PopularEstado(enderecoView), unitOfWork);
                enderecoView.IdCidade = new CidadeService().Salvar(PopularCidade(enderecoView), unitOfWork);
                enderecoView.IdBairro = new BairroService().Salvar(PopularBairro(enderecoView), unitOfWork);

                EnderecoView exit = unitOfWork.EnderecoRepository().Get(enderecoView.IdEndereco);
                if (exit.IdEndereco > 0)
                {
                    enderecoView.IdEndereco = exit.IdEndereco;
                    if (!ValidarAlteracao(enderecoView, exit))
                        unitOfWork.EnderecoRepository().Update(PopularEndereco(enderecoView));
                }
                else
                    unitOfWork.EnderecoRepository().Add(PopularEndereco(enderecoView));

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
           [FromServices] IUnitOfWork unitOfWork, bool interno = false)
        {
            try
            {
                if (id < 1)
                    return new BadRequestObjectResult("Campo de Identificação Endereço Inválido");

                EnderecoView endereco = unitOfWork.EnderecoRepository().Get(id);

                if (string.IsNullOrWhiteSpace(endereco.Cep))
                    return new BadRequestObjectResult("Campo de Identificação Endereço Inválido");

                new EstadoService().Deletar(endereco.IdEstado, unitOfWork);
                new CidadeService().Deletar(endereco.IdCidade, unitOfWork);
                new BairroService().Deletar(endereco.IdCidade, unitOfWork);

                unitOfWork.EnderecoRepository().Remove(id);

               if(!interno)
                    unitOfWork.Commit();
                return new OkResult();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return new BadRequestObjectResult(e.Message);
            }
        }
        public List<EnderecoView> SelecionarEnderecos(
                                [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                return unitOfWork.EnderecoRepository().GetAll();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }

        #region Métodos Auxiliares
        private Bairro PopularBairro(EnderecoView enderecoView)
        {
            return new Bairro
            {
                Id = enderecoView.IdBairro,
                Nome = enderecoView.Bairro,
                IdCidade = enderecoView.IdCidade
            };
        }
        private Cidade PopularCidade(EnderecoView enderecoView)
        {
            return new Cidade
            {
                Id = enderecoView.IdCidade,
                Nome = enderecoView.Localidade,
                IdEstado = enderecoView.IdEstado
            };
        }
        private Estado PopularEstado(EnderecoView endereco)
        {
            return new Estado
            {
                Id = endereco.IdEstado,
                Nome = endereco.NomeEstado,
                Uf = endereco.Uf
            };
        }
        private Endereco PopularEndereco(EnderecoView enderecoView)
        {
            return new Endereco
            {
                Id = enderecoView.IdEndereco,
                IdEstado = enderecoView.IdEstado,
                IdCidade = enderecoView.IdCidade,
                IdBairro = enderecoView.IdBairro,
                Logradouro = enderecoView.Logradouro,
                Complemento = enderecoView.Complemento,
                Cep = enderecoView.Cep
            };
        }
        private bool ValidarAlteracao(EnderecoView endereco, EnderecoView exit)
        {
            if (endereco.Bairro != exit.Bairro)
                return false;
            if (endereco.IdCidade != exit.IdCidade)
                return false;
            if (endereco.IdEstado != exit.IdEstado)
                return false;
            if (endereco.Cep.Trim() != exit.Cep.Trim())
                return false;
            if (endereco.Logradouro.ToUpper() != exit.Logradouro.ToUpper())
                return false;
            if (endereco.Complemento.ToUpper() != exit.Complemento.ToUpper())
                return false;

            return true;
        }
        #endregion
    }
}
