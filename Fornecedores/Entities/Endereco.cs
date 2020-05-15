namespace Fornecedores.Entities
{
   
    public class Endereco
    {
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public int IdBairro { get; set; }
        public int IdEstado { get; set; }
        public int IdCidade { get; set; }
        public string Cep { get; set; }
    }
}
