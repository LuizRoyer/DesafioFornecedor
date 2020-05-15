using System;

namespace Fornecedores.Entities.ViewModels
{
    public class FornecedorView
    {
        public int IdFornecedor { get; set; }
        public DateTime DataCadastro { get; set; }
        public int IdEmpresa { get; set; }
        public string NomeFantasia { get; set; }
        public string CnpjEmpresa { get; set; }
        public int IdEndereco { get; set; }
        public string Cep { get; set; }
        public int IdEstado { get; set; }
        public string Uf { get; set; }
        public string NomeEstado { get; set; }
        public int IdCidade { get; set; }
        public string NomeCidade { get; set; }
        public int IdBairro { get; set; }
        public string NomeBairro { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public int IdPessoa { get; set; }
        public int IdPesFisica { get; set; }
        public int IdPesJuridica { get; set; }
        public string NomeFornecedor { get; set; }
        public string Sobrenome { get; set; }
        public string Tipo { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Rg { get; set; }
        public string Cnpj { get; set; }
    }
}
