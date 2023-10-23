using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _contextUnitOfWork;

        public ProdutosController(IUnitOfWork context)
        {
            _contextUnitOfWork = context;
        }

        //Metodo Action que retorna uma lista de produtos
        //IEnumerable fica mais otimizado
        //ActionResult para retornar mais de um tipo (Pode retornar todos os tipos suportados por ele)
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetAll()
        {
            //Evitar retornar todos os dados, sempre pense em aplicar um filtro = Ex: Take(100)
            return _contextUnitOfWork.ProdutoRepository.Get().Take(100).ToList();
            
        }

        [HttpGet("MaiorPreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return _contextUnitOfWork.ProdutoRepository.GetAllProdutosPreco().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> GetProduto(int id /*[FromQuery]int id]*/ /*[BindRequired]string nome*/)
        {
            //[BindRequired] = Define um parametro obrigatorio; var produtoNome = nome https://localhost:7188/produto/1?nome=Suco
            //[FromQuery] = Mapeia os parametros recebido na query = https://localhost:7188/produto/1?id=2
            //FirstOrDefault caso não encontre o produto retorna null
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            var produto = _contextUnitOfWork.ProdutoRepository.GetByid(prod => prod.ProdutoId == id);

            if(produto is null) 
            {
                return NotFound($"Produto com id: {id} não localizado");
            }
            return produto;
        }

        [HttpPost]
        public async Task<ActionResult> PostProduto([FromBody] Produto produto)
        {
            //[FromBody] e BadRequest são usados de forma implicida pela [ApiController]
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if(produto is null)
            {
                return BadRequest("Dados invalidos");
            }
   
            _contextUnitOfWork.ProdutoRepository.Add(produto); // alocamento em memoria
            _contextUnitOfWork.Commit();

            return new CreatedAtRouteResult("obterproduto", new { id = produto.ProdutoId }, produto);//retorna response status is 201

            //return CreatedAtAction(nameof(GetProduto), new { id = produto.ProdutoId }, produto);
            //return Ok(produto); Tambem é possivel usar. Retorna response status is 200

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<Produto> PutProduto(int id, Produto produto)
        {
            var produtoId = _contextUnitOfWork.ProdutoRepository.GetByid(prod => prod.ProdutoId == id);
            if (id != produto.ProdutoId || produtoId is null)
            {
                return BadRequest($"Não foi possivel atualizar a categoria pois o id = {id} não existe");//retorna response status is 400
            }

            _contextUnitOfWork.ProdutoRepository.Update(produto);
            _contextUnitOfWork.Commit();  
            return Ok(produto);//retorna response status is 200
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<Produto> DeleteProduto(int id)
        {
            var produto = _contextUnitOfWork.ProdutoRepository.GetByid(prod => prod.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                return NotFound($"Não foi possivel excluir a produto pois o id = {id} não existe");
            }
           
            _contextUnitOfWork.ProdutoRepository.Delete(produto);
            _contextUnitOfWork.Commit();

            return Ok(produto);
        }
    }
}
