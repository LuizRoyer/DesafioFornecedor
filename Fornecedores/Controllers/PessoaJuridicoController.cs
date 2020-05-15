using Fornecedores.Entities.ViewEntities;
using Fornecedores.Services;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fornecedores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaJuridicoController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult SavePessoaJuridica(PesJuridicaView pesJuridica,
         [FromServices] IUnitOfWork unitOfWork)
        {
            return new PesJuridicaService().Salvar(pesJuridica, unitOfWork);
        }
        [HttpGet("[action]")]
        public List<PesJuridicaView> SelectPessoasJuridicas([FromServices] IUnitOfWork unitOfWork)
        {
            return new PesJuridicaService().SelecionarPesJuridica(unitOfWork);
        }
        [HttpDelete("[action]")]
        public IActionResult RemovePessoaJuridica(int id,
        [FromServices] IUnitOfWork unitOfWork)
        {
            return new PesJuridicaService().Deletar(id, unitOfWork);
        }
    }
}