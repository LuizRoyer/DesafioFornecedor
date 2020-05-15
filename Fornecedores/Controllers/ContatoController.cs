using Fornecedores.Entities;
using Fornecedores.Services;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fornecedores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult SaveContato(Contato contato,
         [FromServices] IUnitOfWork unitOfWork)
        {
            return new ContatoService().Salvar(contato, unitOfWork);
        }
        [HttpGet("[action]")]
        public List<Contato> SelectContatos(int idPessoa,
         [FromServices] IUnitOfWork unitOfWork)
        {
            return new ContatoService().SelecionarContatos(idPessoa,unitOfWork);
        }
        [HttpDelete("[action]")]
        public IActionResult RemoveContato(int id,
        [FromServices] IUnitOfWork unitOfWork)
        {
            return new ContatoService().Deletar(id, unitOfWork);
        }
    }
}