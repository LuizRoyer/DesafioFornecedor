using System.ComponentModel.DataAnnotations;

namespace Fornecedores.Entities.ViewModels
{
    public class EnderecoView
    {
      
        public int IdEndereco { get; set; }
        public string Cep { get; set; }    
        public int IdEstado { get; set; }
        public string Uf { get; set; }
        public string NomeEstado { get; set; }      
        public int IdCidade { get; set; }
        public string Localidade { get; set; }
        public int IdBairro { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
           
    }
}
