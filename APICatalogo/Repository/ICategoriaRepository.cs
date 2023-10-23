using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetAllCategoriasProdutos();
        IEnumerable<Categoria> GetAllCategoriasProdutosTop100();
    }
}
