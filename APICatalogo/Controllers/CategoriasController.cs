using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
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

        //Ex: de [FromServices]
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices]IMeuServico meuservico, string nome) 
        {
            //Usando [FromServices]
            return meuservico.Saudacao(nome);
        }

        [HttpGet("categoriasQtd")]
        public async Task<ActionResult<string>> GetCategoriasQtd()
        {
            List<Categoria> categorias = new List<Categoria>();
            try
            {
                categorias = await _context.Categorias.ToListAsync();
                if (categorias is null)
                {
                    return BadRequest("Nenhuma categoria cadastrada");
                }
                var quantidade = $"Toda de categorias cadastradas: {categorias.Count<Categoria>().ToString()}";

                return quantidade;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll()
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            //Evitar retornar todos os dados, sempre pense em aplicar um filtro = Ex: Take(100)
            try
            {
                //throw new NotImplementedException();
                return await _context.Categorias.Take(100).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  "Erro ao tentar obter os dados");
            }
     
        }

        [HttpGet("{id:int:min(1)}", Name ="obterCategoria")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            try
            {
                var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(categoria => categoria.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound($"Categoria com o id = {id} não encontrada");
                }

                return Ok(categoria);
                //return categoria; Pode retornar apenas categoria pois o metodo ActionResult<Categoria> dispoem retorno dos metodos 
                //ActionResult e categoria.
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Erro ao tentar obter os dados");
            }
           
        }

        [HttpGet("CategoriaProdutos/{nome}")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllCategoriasProdutosNome(string nome)
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            try
            {
                var categoriaNome = await _context.Categorias.Include(produto => produto.Produtos).AsNoTracking().ToListAsync();
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
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllCategoriasProdutos()
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            //Evitar retornar todos os dados eem obj relacionados, sempre pense em aplicar um filtro = Ex: Where(produto => produto.CategoriaId <= 100)
            try
            { 
                return await _context.Categorias.Include(produto => produto.Produtos)
                       .Where(produto => produto.CategoriaId <= 100).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
          
        }

        [HttpPost]
        public async Task<ActionResult> PostCategoria(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest("Dados invalidos");
                }

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("obterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
           
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> PutCategoria(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    //retorna response status is 400
                    return BadRequest($"Não foi possivel atualizar a categoria pois o id = {id} não existe");
                }

                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(categoria);//retorna response status is 200
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Erro ao tentar obter os dados");
            }
           
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<Categoria>> DeleteCategoria(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Categoria não encontrada para efetuar a exclusãp");
                }

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();

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
