using APICatalogo.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        //Boa pratica inicializar a coleção que está incluida na classe 
        public Categoria()
        {
            Produtos = new Collection<Produto>();    
        }

        [Key]
        public int CategoriaId { get; set; }//chave primaria

        [Required(ErrorMessage = "Nome obrigatório")]
        [MaxLength(80)]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "O nome deve ter entre {2} e {1} caracteres")]
        [PrimeiraLetraMaiuscula] //Atributo customizado
        public string? Nome { get; set; }

        [Required(ErrorMessage = "ImagemUrl obrigatória")]
        [MaxLength(300)]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "A ImagemUrl deve ter entre {2} e {1} caracteres")]
        public string? ImagemUrl { get; set;  }

        //Propriedade de navegação 1 x N - Propriedades de navegação não são mapeadas(Não cria coluna) no BD
        //EF Mapeia e cria uma FK CategoriaId em Produtos
  
        public ICollection<Produto>? Produtos { get; set; }

    }

}
