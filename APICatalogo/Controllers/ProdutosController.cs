using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao(string nome)
        {
            //Sem a utilização [FromServices]
            MeuServico meuservico = new MeuServico();
            return meuservico.Saudacao(nome);
           
        }

        //Metodo Action que retorna uma lista de produtos
        [HttpGet]
        //IEnumerable fica mais otimizado
        //ActionResult para retornar mais de um tipo (Pode retornar todos os tipos suportados por ele)
        public async Task<ActionResult<IEnumerable<Produto>>> GetAll()
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            //Evitar retornar todos os dados, sempre pense em aplicar um filtro = Ex: Take(100)
            var produtos = await _context.Produtos.Take(100).AsNoTracking().ToListAsync();
            if(produtos is null)
            {
                return NotFound("Produtos não encontrados");//Retorno response status is 404 - Herda de ActionResult
            }
            return produtos;
        }

        [HttpGet("PrimeiroProduto")]

        public async Task<ActionResult<Produto>> GetPrimeiroProduto()
        {
            var produtos = await _context.Produtos.FirstOrDefaultAsync<Produto>();
            if (produtos is null)
            {
                return NotFound("Produto não encontrado");//Retorno response status is 404 - Herda de ActionResult
            }
            return produtos;
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public async Task<ActionResult<Produto>> GetProduto(int id /*[FromQuery]int id]*/ /*[BindRequired]string nome*/)
        {
            //[BindRequired] = Define um parametro obrigatorio; var produtoNome = nome https://localhost:7188/produto/1?nome=Suco
            //[FromQuery] = Mapeia os parametros recebido na query = https://localhost:7188/produto/1?id=2
            //FirstOrDefault caso não encontre o produto retorna null
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync<Produto>(
                produto => produto.ProdutoId == id);

            if(produto is null) 
            {
                return NotFound($"Produto com o id = {id} não encontrada");
            }
            return produto;
        }

        [HttpPost]
        public async Task<ActionResult> PostProduto([FromBody] Produto produto)
        {
            //[FromBody] e BadRequest são usados de forma implicida pela [ApiController]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(produto is null)
            {
                return BadRequest("Dados invalidos");
            }
   
            _context.Produtos.Add(produto); // alocamento em memoria
            await _context.SaveChangesAsync(); // metodo para persistir no bando de dados

            return new CreatedAtRouteResult("obterproduto", new { id = produto.ProdutoId }, produto);//retorna response status is 201

            //return CreatedAtAction(nameof(GetProduto), new { id = produto.ProdutoId }, produto);
            //return Ok(produto); Tambem é possivel usar. Retorna response status is 200

        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> PutProduto(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest($"Não foi possivel atualizar a categoria pois o id = {id} não existe");//retorna response status is 400
            }

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(produto);//retorna response status is 200
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync<Produto>(produto => produto.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                return NotFound($"Não foi possivel excluir a produto pois o id = {id} não existe");
            }
           
            _context.Produtos.Remove(produto); 
            await _context.SaveChangesAsync();

            return Ok(produto);
        }
    }
}
