namespace Fornecedores.Entities.ViewEntities
{
    public class PesJuridicaView
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }       
        public string Cnpj { get; set; }
    }
}
