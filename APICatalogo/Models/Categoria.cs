using APICatalogo.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{  
    public class Categoria
    {
        //Boa pratica inicializar a coleção que está incluida na classe 
        public Categoria()
        {
            Produtos = new Collection<Produto>();    
        }

        public int CategoriaId { get; set; }//chave primaria
        public string? Nome { get; set; }
        public string? ImagemUrl { get; set;  }

        //Propriedade de navegação 1 x N - Propriedades de navegação não são mapeadas(Não cria coluna) no BD
        //EF Mapeia e cria uma FK CategoriaId em Produtos
        public ICollection<Produto>? Produtos { get; set; }

    }

}
