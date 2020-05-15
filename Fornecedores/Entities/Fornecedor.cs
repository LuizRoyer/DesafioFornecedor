using System;

namespace Fornecedores.Entities
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdPessoa { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
