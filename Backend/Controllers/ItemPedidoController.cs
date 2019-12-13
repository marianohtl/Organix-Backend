using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class ItemPedidoController : ControllerBase
    {
        // organixContext _contexto = new organixContext();

        ItemPedidoRepository _repositorio = new ItemPedidoRepository();

        // GET : api/ItemPedido
        /// <summary>
        /// Consulta os Itens dos pedidos
        /// </summary>
        /// <returns>Retornar uma lista de itens cadastrados</returns>
        [HttpGet]
        public async Task<ActionResult<List<ItemPedido>>> Get(){

            var itemPedidos = await _repositorio.Listar();

            if(itemPedidos == null){
                return NotFound(new {mensagem = "Não foi possível localizar o produto."});
            }

            return itemPedidos;

        }

        // GET : api/ItemPedido2
        /// <summary>
        /// Consulta os itens dos pedidos pelo ID
        /// </summary>
        /// <returns>Retorna o item referenciado pelo id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemPedido>> Get(int id){

            // FindAsync = procura algo específico no banco
            var itemPedido = await _repositorio.BuscarPorId(id);

            if(itemPedido == null){
                return NotFound(new{mensagem = "Não foi possível localizar o item pedido."});
            }

            return itemPedido;

        }

        // POST api/ItemPedido
        /// <summary>
        /// Cria um novo item do pedido
        /// </summary>
        /// <returns>Envia para o banco os itens cadastrados </returns>
        [HttpPost]
        public async Task<ActionResult<ItemPedido>> Post(ItemPedido itemPedido){

            try
            {
                await _repositorio.Salvar(itemPedido);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }

            return itemPedido;
        }
        /// <summary>
        ///  Altera o item do pedido pelo ID
        /// </summary>
        /// <returns>Envia para o banco o item alterado</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ItemPedido itemPedido){
            // Se o id do objeto não existir, ele retorna erro 400
            if(id != itemPedido.IdItemPedido){
                return BadRequest(new{mensagem = "Não foi possivel localizar o item pedido."});
            }
            
            

            try
            {

                await _repositorio.Alterar(itemPedido);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificamos se o objeto inserido realmente existe no banco
                var itemPedido_valido = await _repositorio.BuscarPorId(id);

                if(itemPedido_valido == null){
                    return NotFound(new{mensagem = "Pedido inexistente."});
                }else{

                throw;
                }

                
            }
            // NoContent = retorna 204, sem nada
            return NoContent();
        }

        // DELETE api/itemPedido/id
        /// <summary>
        /// Deleta o item do pedido cadastrado
        /// </summary>
        /// <returns> Deleta um item cadastrado</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemPedido>> Delete(int id){
            var itemPedido = await _repositorio.BuscarPorId(id);
            if(itemPedido == null){
                return NotFound(new{mensagem = "Não foi possivel deletar o pedido."});
            }
            await _repositorio.Excluir(itemPedido);
            
            return itemPedido;
        }
    }
}