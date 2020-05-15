using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using Fornecedores.Services;
using Fornecedores.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fornecedores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult SaveEmpresa(Empresa empresa,
          [FromServices] IUnitOfWork unitOfWork)
        {
            return new EmpresaService().Salvar(empresa, unitOfWork);
        }
        [HttpGet("[action]")]
        public List<EmpresaView> SelectEmpresas(
         [FromServices] IUnitOfWork unitOfWork)
        {
            return new EmpresaService().SelecionarEmpresas(unitOfWork);
        }
        [HttpDelete("[action]")]
        public IActionResult RemoveEmpresa(int id,
        [FromServices] IUnitOfWork unitOfWork)
        {
            return new EmpresaService().Deletar(id, unitOfWork);
        }
    }
}