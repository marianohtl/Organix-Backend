using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using backend.Repositories;
using Backend.Domains;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    // Definimos nossa rota do controller e dizemos que é um controller de API
    [Authorize(Roles="1,2,3")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        // GufosContext _contexto = new GufosContext();

        ProdutoRepository _repositorio = new ProdutoRepository();
        UploadRepository _upRepositorio = new UploadRepository();

        // GET : api/Produto
        /// <summary>
        /// Consuta uma lista de produtos cadastrados
        /// </summary>
        /// <returns>Retorna uma lista de produtos</returns>
        [Authorize(Roles="1,2,3")]
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> Get(){

            var produtos = await _repositorio.Listar();

            if(produtos == null){
                return NotFound(new {mensagem = "Nenhum produto foi encontrado."});
                
            }

            return produtos;
        }

        // GET : api/Produto2
        /// <summary>
        /// Consulta produto baseado no ID
        /// </summary>
        /// <returns>Retorna um produto </returns>
        [Authorize(Roles="1,2,3")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> Get(int id){

            // FindAsync = procura algo específico no banco
            var produto = await _repositorio.BuscarPorId(id);

            if(produto == null){
                return NotFound(new {mensagem = "Nenhum produto foi encontrado."});
            }

            return produto;
        }

        // POST api/Produto
        /// <summary>
        /// Cadastra um novo produto pelo usuário administrador
        /// </summary>
        /// <returns>Cadastra um novo produto</returns>
        [Authorize(Roles="1")]
        [HttpPost]
        public async Task<ActionResult<Produto>> Post([FromForm]Produto produto){
  
                   var arquivo = Request.Form.Files[0];
                    produto.Imagem =  _upRepositorio.Upload(arquivo,"Resources/Images");
                    await _repositorio.Salvar(produto);

            return produto;
        }
        /// <summary>
        /// Altera produto baseado no ID
        /// </summary>
        /// <returns>Envia alteração do produto no banco </returns>
        [Authorize(Roles="1")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,[FromForm]Produto produto){
            // Se o id do objeto não existir, ele retorna erro 400
            if(id != produto.IdProduto){
                return NotFound(new {mensagem = "Não é possível alterar este produto."});
            }
           
            try
            {
               var arquivo = Request.Form.Files[0];
                produto.Imagem =  _upRepositorio.Upload(arquivo,"Resources/Images");
                await _repositorio.Alterar(produto);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificamos se o objeto inserido realmente existe no banco
                var produto_valido = await _repositorio.BuscarPorId(id);

                if(produto_valido == null){
                    return NotFound(new {mensagem = "Não é possível alterar este produto."});  
                }else{

                throw;
                }

                
            }
            // NoContent = retorna 204, sem nada
            return NoContent();
        }

        // DELETE api/produto/id
        /// <summary>
        /// Deleta produto
        /// </summary>
        /// <returns>Deleta o produto no banco</returns>
        [Authorize(Roles="1")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Produto>> Delete(int id){
            var produto = await _repositorio.BuscarPorId(id);
            if(produto == null){
                return NotFound(new {mensagem = "Produto inexistente."});  
            }
            await _repositorio.Excluir(produto);
            
                   
            return produto;
        }
    }
}