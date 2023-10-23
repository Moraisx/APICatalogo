using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _contextUnitOfWork;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork context, ILogger<CategoriasController> logger)
        {
            _contextUnitOfWork = context;
            _logger = logger;
        }

        //Ex: de [FromServices]
        //[HttpGet("saudacao/{nome}")]
        //public ActionResult<string> GetSaudacao([FromServices]IMeuServico meuservico, string nome) 
        //{
        //    //Usando [FromServices]
        //    return meuservico.Saudacao(nome);
        //}

        [HttpGet("SimularError")]
        public ActionResult<string> GetConfigureExceptionHandler()
        {
            //throw new Exception("Exception de error no retorno.;  configuração de error global ConfigureExceptionHandler()");

            string[] error = null;
            if(error.Length > 0)
            {
                return "True";
            }

            return "Error";
        }

        [HttpGet("categoriasQtd")]
        public ActionResult<string> GetCategoriasQtd()
        {
            List<Categoria> categorias = new List<Categoria>();
            try
            {
                categorias = _contextUnitOfWork.CategoriaRepository.Get().ToList();
                if (categorias is null)
                {
                    return BadRequest("Nenhuma categoria cadastrada");
                }
                var quantidade = $"Toda de categorias cadastradas: {categorias.Count()}";

                return quantidade;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            //Evitar retornar todos os dados, sempre pense em aplicar um filtro = Ex: Take(100)

            try
            {
                return _contextUnitOfWork.CategoriaRepository.Get().Take(100).ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  "Erro ao tentar obter os dados");
            }
     
        }

        [HttpGet("{id:int:min(1)}", Name ="obterCategoria")]
        public ActionResult<Categoria> GetCategoria(int id)
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            try
            {
                var categoria = _contextUnitOfWork.CategoriaRepository.GetByid(categ => categ.CategoriaId == id);

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
        public ActionResult<IEnumerable<Categoria>> GetAllCategoriasProdutosNome(string nome)
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            try
            {
                var categoriaNome = _contextUnitOfWork.CategoriaRepository.GetAllCategoriasProdutos();
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

        [HttpGet("produtosTop100")]
        public ActionResult<IEnumerable<Categoria>> GetAllCategoriasProdutos()
        {
            //AsNoTracking() melhora o desempenho, usado somente em consultas de leitura - Get()
            //Evitar retornar todos os dados eem obj relacionados, sempre pense em aplicar um filtro = Ex: Where(produto => produto.CategoriaId <= 100)
            try
            {
                return _contextUnitOfWork.CategoriaRepository.GetAllCategoriasProdutosTop100().ToList();
                       
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                 "Erro ao tentar obter os dados");
            }
          
        }

        [HttpPost]
        public ActionResult<Categoria> PostCategoria(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest("Dados invalidos");
                }

                _contextUnitOfWork.CategoriaRepository.Add(categoria);
                _contextUnitOfWork.Commit();

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
            var categoriaId = _contextUnitOfWork.CategoriaRepository.GetByid(categ => categ.CategoriaId == id);
            try
            {
                if (id != categoria.CategoriaId || categoriaId is null)
                {
                    //retorna response status is 400
                    return BadRequest($"Não foi possivel atualizar a categoria pois o id = {id} não existe");
                }

                _contextUnitOfWork.CategoriaRepository.Update(categoria);
                _contextUnitOfWork.Commit();

                return Ok(categoria);//retorna response status is 200
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Erro ao tentar obter os dados");
            }
           
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<Categoria> DeleteCategoria(int id)
        {
            try
            {
                var categoria = _contextUnitOfWork.CategoriaRepository.GetByid(categoria => categoria.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Categoria não encontrada para efetuar a exclusãp");
                }

                _contextUnitOfWork.CategoriaRepository.Delete(categoria);
                _contextUnitOfWork.Commit();

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
