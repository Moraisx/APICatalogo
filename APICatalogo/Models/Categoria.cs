using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        [Required]
        [MaxLength(80)]
        public string? Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string? ImagemUrl { get; set;  }

        //Propriedade de navegação 1 x N - Propriedades de navegação não são mapeadas(Não cria coluna) no BD
        //EF Mapeia e cria uma FK CategoriaId em Produtos
        public ICollection<Produto>? Produtos { get; set; }

    }

}
