namespace Fornecedores.Entities.ViewModels
{
    public class EmpresaView
    {
        public int IdEmpresa { get; set; }
        public string NomeFantasia { get; set; }        
        public string CNPJ { get; set; }
        public EnderecoView Endereco { get; set; }
    }
}
