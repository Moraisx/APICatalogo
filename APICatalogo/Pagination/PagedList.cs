namespace APICatalogo.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int PaginaAtual { get; set; }
        public int TodalDePaginas { get; set; }
        public int TamanhoDaPagina { get; set; }
        public int ContagemTotal { get; set; }

        public bool PaginaAnterior  => PaginaAtual > 1;
        public bool PaginaPosterior => PaginaAtual < TodalDePaginas;

        public PagedList(List<T> items, int contagemTotal, int numeroDaPagina, int tamanhoDaPagina)
        {
            ContagemTotal = contagemTotal;
            PaginaAtual = numeroDaPagina;
            TamanhoDaPagina = tamanhoDaPagina;
            TodalDePaginas = (int)Math.Ceiling(contagemTotal/(double)tamanhoDaPagina);

            AddRange(items);
        }

        public static PagedList<T> ListaDePaginas(IQueryable<T> dados, int numeroDaPagina, int tamanhoDaPagina)
        {
            var count = dados.Count();
            var items = dados.Skip((numeroDaPagina - 1) * tamanhoDaPagina).Take(tamanhoDaPagina).ToList();
            
            return new PagedList<T>(items, count, numeroDaPagina, tamanhoDaPagina);
        }
    }
}
