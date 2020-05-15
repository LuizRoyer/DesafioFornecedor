using Fornecedores.IRepositories;

namespace Fornecedores.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IEstadoRepository EstadoRepository();
        IEnderecoRepository EnderecoRepository();
        ICidadeRepository CidadeRepository();
        IBairroRepository BairroRepository();
        IEmpresaRepository EmpresaRepository();
        IPessoaRepository PessoaRepository();
        IPesFisicaRepository PesFisicaRepository();
        IPesJuridicaRepository PesJuridicaRepository();
        IContatoRepository ContatoRepository();
        IFornecedorRepository FornecedorRepository();

        void Commit();
        void Rollback();
    }
}
