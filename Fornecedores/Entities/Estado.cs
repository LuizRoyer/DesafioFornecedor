using System.ComponentModel.DataAnnotations;

namespace Fornecedores.Entities
{

    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Uf { get; set; }
    }
}
