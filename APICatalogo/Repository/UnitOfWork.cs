using APICatalogo.Context;

namespace APICatalogo.Repository
{
    //Registrar no program o serviço
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepository;
        private CategoriaRepository _categoriaRepository;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository 
        {
            get 
            {
                //Valida is null
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                //Valida is null
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
            }
        }

        public void Commit()
        {
            // Persiste no BD
            _context.SaveChanges();
        }

        public void Dispose()
        {
            //libera recursos na injeção do contexto
            _context.Dispose();
        }
    }
}
