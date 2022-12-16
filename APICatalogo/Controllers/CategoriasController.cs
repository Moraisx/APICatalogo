using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("CategoriaProdutos/{nome}")]
        public ActionResult<IEnumerable<Categoria>> GetAllCategoriasProdutosNome(string nome)
        {

            var categoriaNome = _context.Categorias.Include(produto => produto.Produtos).ToList();
            var categoriaProdutos = categoriaNome.FirstOrDefault(categoria_nome => categoria_nome.Nome == nome);

            if (categoriaProdutos is null)
            {
                return NotFound("Categoria não encontrada");
            }

            return Ok(categoriaProdutos);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetAllCategoriasProdutos()
        {
            return _context.Categorias.Include(produto => produto.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            return _context.Categorias.ToList();
        }

        [HttpGet("{id:int}", Name ="obterCategoria")]
        public ActionResult GetCategoria(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == id);

            if(categoria is null)
            {
                return NotFound("Categoria não encontrada");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult PostCategoria(Categoria categoria)
        {
            if(categoria is null) 
            {
                return BadRequest("Categoria não pode ser nula");
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("obterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest("categoria não encontrada");//retorna response status is 400
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);//retorna response status is 200
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> DeleteCategoria(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == id);

            if(categoria is null)
            {
                return NotFound("Categoria não encontrada para efetuar a exclusãp");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
