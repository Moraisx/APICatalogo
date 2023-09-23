using APICatalogo.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        [MaxLength(80)]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "O nome deve ter entre {2} e {1} caracteres")]
        [PrimeiraLetraMaiuscula] //Atributo customizado
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Descricao obrigatória")]
        [MaxLength(300)]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "A Descricao deve ter entre {2} e {1} caracteres")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Preco obrigatório")]
        [Column(TypeName = "decimal(10,2)")] //coluna do tipo decimal(10,2)
        [Range(1, 1000000, ErrorMessage = "O preço deve estar entre {1} a {2}")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "ImagemUrl obrigatória")]
        [MaxLength(300)]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "A ImagemUrl deve ter entre {2} e {1} caracteres")]
        public string? ImagemUrl { get; set; }

        [Required(ErrorMessage = "Estoque obrigatório")]
        public float? Estoque { get; set; }

        public DateTime DataCadastro { get; set; }

        //Mapeia para FK no BD
        public int CategoriaId { get; set; }

        //indica que Produto está relacionado com Categoria - propriedade de navegação
        [JsonIgnore]//Ignora essa propriedade na serialização, Ex: _context.Categorias.Include(produto => produto.Produtos).AsNoTracking().ToListAsync();
        public Categoria? Categoria{ get; set; }

    }
}
