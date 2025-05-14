namespace CadastroASP.NET.Models
{
    public class Produto
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public double? Preco { get; set; }
        public int? Quantidade { get; set; }
        public List<Produto>? ListaProduto { get; set; }
    }
}