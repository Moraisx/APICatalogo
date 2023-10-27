using APICatalogo.Pagination;

namespace APICatalogo.DTOs
{
    public class PaginacaoDTO<T> where T : class
    {
        public int PaginaAtual { get; set; }
        public int TodalDePaginas { get; set; }
        public int TamanhoDaPagina { get; set; }
        public int ContagemTotal { get; set; }

        public bool PaginaAnterior { get; set; }
        public bool PaginaPosterior { get; set; }
        public List<T> model { get; set; }

        public PaginacaoDTO(int paginaAtual, int todalDePaginas, int tamanhoDaPagina, int contagemTotal, bool paginaAnterior, bool paginaPosterior)
        {
            PaginaAtual = paginaAtual;
            TodalDePaginas = todalDePaginas;
            TamanhoDaPagina = tamanhoDaPagina;
            ContagemTotal = contagemTotal;
            PaginaAnterior = paginaAnterior;
            PaginaPosterior = paginaPosterior;    
        }

        public static object PaginacaoModel(PaginacaoDTO<T> paginacaoDTO, List<T> model)
        {
            return new
            {
                Pagina = paginacaoDTO.PaginaAtual,
                Items_por_pagina = paginacaoDTO.TamanhoDaPagina,
                Total_Paginas = paginacaoDTO.TodalDePaginas,
                Total_registros = paginacaoDTO.ContagemTotal,
                NextPage = paginacaoDTO.PaginaPosterior,
                PreviewPage = paginacaoDTO.PaginaAnterior,
                data = model
            };

        }
    }
}
