using System;

namespace Fornecedores.Entities
{
    public class PesFisica
    {
        public int Id { get; set; }        
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Rg { get; set; }
        public int IdPessoa { get; set; }
    }
}
