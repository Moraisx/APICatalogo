using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _contextUnitOfWork;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _contextUnitOfWork = context;
            _mapper = mapper;
        }

        //Metodo Action que retorna uma lista de produtos
        //IEnumerable fica mais otimizado
        //ActionResult para retornar mais de um tipo (Pode retornar todos os tipos suportados por ele)
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAllProdutosPaginados([FromQuery] ProdutosParameters produtosParameters)
        {
            var produto = _contextUnitOfWork.ProdutoRepository.GetAllProdutosPaginados(produtosParameters);
            var metadata = new
            {
                produto.TodalDePaginas,
                produto.TamanhoDaPagina,
                produto.PaginaAtual,
                produto.ContagemTotal,
                produto.PaginaAnterior,
                produto.PaginaPosterior,
            };

            Response.Headers.Add("Paginacao", JsonConvert.SerializeObject(metadata));

            var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produto);

            return Ok(new
            {
                Pagina = produto.PaginaAtual,
                Items_por_pagina = produto.TamanhoDaPagina,
                Total_Paginas = produto.TodalDePaginas,
                Total_registros = produto.ContagemTotal,
                data = produtoDTO
            });

            //Sem auto-mapper
            /* var produto = _contextUnitOfWork.ProdutoRepository.Get().Take(100).ToList();
            var produtoDTO = new List<ProdutoDTO>();
            foreach (var prod in produto)
            {
                produtoDTO.Add(new ProdutoDTO
                {
                    ProdutoId = prod.ProdutoId,
                    Nome = prod.Nome,
                    Descricao = prod.Descricao,
                    Preco = prod.Preco,
                    ImagemUrl = prod.ImagemUrl,
                    CategoriaId = prod.CategoriaId
                }) ;
            }
            return produtoDTO; */
        }

        [HttpGet("TodosProdutos")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetAll()
        {
            //Evitar retornar todos os dados, sempre pense em aplicar um filtro = Ex: Take(100)
            var produto = _contextUnitOfWork.ProdutoRepository.Get().Take(100).ToList();
            var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produto);
            return produtoDTO;

            //Sem auto-mapper
            /* var produto = _contextUnitOfWork.ProdutoRepository.Get().Take(100).ToList();
            var produtoDTO = new List<ProdutoDTO>();
            foreach (var prod in produto)
            {
                produtoDTO.Add(new ProdutoDTO
                {
                    ProdutoId = prod.ProdutoId,
                    Nome = prod.Nome,
                    Descricao = prod.Descricao,
                    Preco = prod.Preco,
                    ImagemUrl = prod.ImagemUrl,
                    CategoriaId = prod.CategoriaId
                }) ;
            }
            return produtoDTO; */
        }

        [HttpGet("MaiorPreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            
            var produto = _contextUnitOfWork.ProdutoRepository.GetAllProdutosPreco().ToList();
            var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produto);
            return produtoDTO;
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<ProdutoDTO> GetProduto(int id /*[FromQuery]int id]*/ /*[BindRequired]string nome*/)
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

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return produtoDTO;
        }

        [HttpPost]
        public async Task<ActionResult> PostProduto([FromBody] ProdutoDTO produtoDto)
        {
            //[FromBody] e BadRequest são usados de forma implicida pela [ApiController]
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //Para POST converter o para tipo Produto
            var produto = _mapper.Map<Produto>(produtoDto);

            if(produto is null)
            {
                return BadRequest("Dados invalidos");
            }
   
            _contextUnitOfWork.ProdutoRepository.Add(produto); // alocamento em memoria
            _contextUnitOfWork.Commit();

            var resultProdutoDTO = _mapper.Map<ProdutoDTO>(produto); 
            return new CreatedAtRouteResult("obterproduto", new { id = produto.ProdutoId }, resultProdutoDTO);//retorna response status is 201

            //return CreatedAtAction(nameof(GetProduto), new { id = produto.ProdutoId }, produto);
            //return Ok(produto); Tambem é possivel usar. Retorna response status is 200

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<Produto> PutProduto(int id, ProdutoDTO produtoDto)
        {
            var produtoId = _contextUnitOfWork.ProdutoRepository.GetByid(prod => prod.ProdutoId == id);
            if (id != produtoDto.ProdutoId || produtoId is null)
            {
                return BadRequest($"Não foi possivel atualizar a categoria pois o id = {id} não existe");//retorna response status is 400
            }

            var produto = _mapper.Map<Produto>(produtoDto);
            _contextUnitOfWork.ProdutoRepository.Update(produto);
            _contextUnitOfWork.Commit();

            //retorna response status is 200
            return Ok(produto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<ProdutoDTO> DeleteProduto(int id)
        {
            var produto = _contextUnitOfWork.ProdutoRepository.GetByid(prod => prod.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                return NotFound($"Não foi possivel excluir a produto pois o id = {id} não existe");
            }
           
            _contextUnitOfWork.ProdutoRepository.Delete(produto);
            _contextUnitOfWork.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDTO);
        }
    }
}
