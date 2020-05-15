using Fornecedores.Entities.ViewEntities;
using Fornecedores.Services;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fornecedores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaFisicaController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult SavePessoaFisica(PesFisicaView pesFisica,
            [FromServices] IUnitOfWork unitOfWork)
        {
            return new PesFisicaService().Salvar(pesFisica, unitOfWork);
        }
        [HttpGet("[action]")]
        public List<PesFisicaView> SelectPessoaFisicas([FromServices] IUnitOfWork unitOfWork)
        {
            return new PesFisicaService().SelecionarPesFisica(unitOfWork);
        }
        [HttpDelete("[action]")]
        public IActionResult RemovePessoaFisica(int id,
        [FromServices] IUnitOfWork unitOfWork)
        {
            return new PesFisicaService().Deletar(id, unitOfWork);
        }
    }
}