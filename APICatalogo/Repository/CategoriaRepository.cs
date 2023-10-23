using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Categoria> GetAllCategoriasProdutos()
        {
            return Get().Include(produto => produto.Produtos).AsNoTracking().ToList();
        }

        public IEnumerable<Categoria> GetAllCategoriasProdutosTop100()
        {
            return Get().Include(produto => produto.Produtos).Where(categoria =>  categoria.CategoriaId <= 100).AsNoTracking().ToList();
        }
    }
}
