using APICatalogo.Context;
using APICatalogo.Models;
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

        //Metodo Action que retorna uma lista de produtos
        [HttpGet]
        //IEnumerable fica mais otimizado
        //ActionResult para retornar mais de um tipo (Pode retornar todos os tipos suportados por ele)
        public ActionResult<IEnumerable<Produto>> GetAll()
        {
            var produtos = _context.Produtos.ToList();
            if(produtos is null)
            {
                return NotFound("Produtos não encontrados");//Retorno response status is 404 - Herda de ActionResult
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name="ObterProduto")]
        public ActionResult<Produto> GetProduto(int id)
        {
            //FirstOrDefault caso não encontre o produto retorna null
            var produto = _context.Produtos.FirstOrDefault<Produto>(
                produto => produto.ProdutoId == id);

            if(produto is null) 
            {
                return NotFound("Produto não encontrado");
            }
            return produto;
        }

        [HttpPost]
        public ActionResult PostProduto([FromBody] Produto produto)
        {
            //[FromBody] e BadRequest são usados de forma implicida pela [ApiController]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(produto is null)
            {
                return BadRequest();
            }
   
            _context.Produtos.Add(produto); // alocamento em memoria
            _context.SaveChanges(); // metodo para persistir no bando de dados

            return new CreatedAtRouteResult("obterproduto", new { id = produto.ProdutoId }, produto);//retorna response status is 201

            //return CreatedAtAction(nameof(GetProduto), new { id = produto.ProdutoId }, produto);
            //return Ok(produto); Tambem é possivel usar.

        }

        [HttpPut("{id:int}")]
        public ActionResult PutProduto(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest("Produto não encontrado");//retorna response status is 400
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);//retorna response status is 200
        }

        [HttpDelete("{id:int}")]
        public  ActionResult DeleteProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault<Produto>(produto => produto.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado para efetuar a exclusão");
            }
           
            _context.Produtos.Remove(produto); 
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
