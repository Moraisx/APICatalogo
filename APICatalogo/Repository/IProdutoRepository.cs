﻿using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
       PagedList<Produto> GetAllProdutosPaginados(ProdutosParameters produtosParameters);
       IEnumerable<Produto> GetAllProdutosPreco();
    }
}
