using System;
namespace Fornecedores.Entities.ViewEntities
{
    public class PesFisicaView
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Rg { get; set; }    
    }
}
