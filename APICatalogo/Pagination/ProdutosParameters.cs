using System.Security.AccessControl;

namespace APICatalogo.Pagination
{
    public class ProdutosParameters
    {
        private int _tamanhoDaPagina = 10;
        const int tamanhoMaximo = 50;
        public int NumeroDaPagina { get; set; } = 1;
        public int TamanhoDaPagina
        {
            get
            {
                return _tamanhoDaPagina;
            }
            set
            {
                _tamanhoDaPagina = (value > tamanhoMaximo) ? tamanhoMaximo : value;
            }
        }
    }
}
