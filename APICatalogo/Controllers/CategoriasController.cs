using APICatalogo.Context;
using APICatalogo.Models;
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

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            //Evitar retornar todos os dados, sempre pense em aplicar um filtro = Ex: Take(100)
            try
            {
                //throw new NotImplementedException();
                return _context.Categorias.Take(100).AsNoTracking().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  "Erro ao tentar obter os dados");
            }
     
        }

        [HttpGet("{id:int}", Name ="obterCategoria")]
        public ActionResult GetCategoria(int id)
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            try
            {
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(categoria => categoria.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound($"Categoria com o id = {id} não encontrada");
                }

                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Erro ao tentar obter os dados");
            }
           
        }

        [HttpGet("CategoriaProdutos/{nome}")]
        public ActionResult<IEnumerable<Categoria>> GetAllCategoriasProdutosNome(string nome)
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            try
            {
                var categoriaNome = _context.Categorias.Include(produto => produto.Produtos).AsNoTracking().ToList();
                var categoriaProdutos = categoriaNome.FirstOrDefault(categoria_nome => categoria_nome.Nome == nome);

                if (categoriaProdutos is null)
                {
                    return NotFound($"Categoria com o nome = {nome} não encontrada");
                }

                return Ok(categoriaProdutos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
          
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetAllCategoriasProdutos()
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            //Evitar retornar todos os dados eem obj relacionados, sempre pense em aplicar um filtro = Ex: Where(produto => produto.CategoriaId <= 100)
            try
            { 
                return _context.Categorias.Include(produto => produto.Produtos)
                       .Where(produto => produto.CategoriaId <= 100).AsNoTracking().ToList();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
          
        }

        [HttpPost]
        public ActionResult PostCategoria(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest("Dados invalidos");
                }

                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                return new CreatedAtRouteResult("obterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
           
        }

        [HttpPut("{id:int}")]
        public ActionResult PutCategoria(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    //retorna response status is 400
                    return BadRequest($"Não foi possivel atualizar a categoria pois o id = {id} não existe");
                }

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(categoria);//retorna response status is 200
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Erro ao tentar obter os dados");
            }
           
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> DeleteCategoria(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Categoria não encontrada para efetuar a exclusãp");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
           
        }
    }
}
