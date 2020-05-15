using Fornecedores.Entities.ViewModels;
using Fornecedores.Services;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fornecedores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult SaveEndereco(EnderecoView endereco,           
            [FromServices] IUnitOfWork unitOfWork)
        {
            return new EnderecoService().Salvar(endereco,unitOfWork);
        }
        [HttpGet("[action]")]
        public List<EnderecoView> SelectEnderecos(         
         [FromServices] IUnitOfWork unitOfWork)
        {
            return new EnderecoService().SelecionarEnderecos(unitOfWork);
        }
        [HttpDelete("[action]")]
        public IActionResult RemoveEndereco( int id,      
        [FromServices] IUnitOfWork unitOfWork)
        {
            return new EnderecoService().Deletar(id, unitOfWork);
        }      
    }
}