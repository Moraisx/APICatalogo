using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Produto> GetAllProdutosPaginados(ProdutosParameters produtosParameters)
        {
            return PagedList<Produto>.ListaDePaginas(Get().OrderBy(on => on.ProdutoId),
                produtosParameters.NumeroDaPagina, produtosParameters.TamanhoDaPagina);
        }

        public IEnumerable<Produto> GetAllProdutosPreco()
        {
            return Get().OrderByDescending(produto => produto.Preco).AsNoTracking();
        }
    }

}
