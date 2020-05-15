using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using Fornecedores.Services;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Fornecedores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult SaveFornecedor(Fornecedor fornecedor,
           [FromServices] IUnitOfWork unitOfWork)
        {
            return new FornecedorService().Salvar(fornecedor, unitOfWork);
        }
        [HttpGet("[action]")]
        public List<FornecedorView> SelectFornecedores(string Nome_Pessoa, string CPF, string CNPJ_Empresa, DateTime dataCadastro,
         [FromServices] IUnitOfWork unitOfWork)
        {
            return new FornecedorService().SelecionarFornecedores(Nome_Pessoa, CPF, CNPJ_Empresa, dataCadastro,unitOfWork);
        }
        [HttpDelete("[action]")]
        public IActionResult RemoveFornecedor(int id,
        [FromServices] IUnitOfWork unitOfWork)
        {
            return new FornecedorService().Deletar(id, unitOfWork);
        }
    }
}