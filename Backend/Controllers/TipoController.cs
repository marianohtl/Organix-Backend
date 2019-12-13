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
    public class TipoController : ControllerBase
    {
        // organixContext _contexto = new organixContext();

        TipoRepository _repositorio = new TipoRepository();

        // GET : api/Tipo
        /// <summary>
        /// Consulta o tipo do usuário
        /// </summary>
        /// <returns>Retorna um valor válido</returns>
        [HttpGet]
        public async Task<ActionResult<List<Tipo>>> Get(){

            var tipos = await _repositorio.Listar();

            if(tipos == null){
                return NotFound(new {mensagem = "Nenhum tipo foi encontrado, verifique e tente novamente!"});
            }

            return tipos;

        }

        // GET : api/Tipo2
        /// <summary>
        /// Consulta um tipo de usuário baseado no ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna um tipo tipo de usuário se for válido</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Tipo>> Get(int id){

            // FindAsync = procura algo específico no banco
            var tipo = await _repositorio.BuscarPorId(id);

            if(tipo == null){
                return NotFound(new {mensagem = "Nenhum tipo foi encontrado para o ID informado, verifique e tente novamente!"});
            }

            return tipo;

        }

        // POST api/Tipo
        /// <summary>
        /// Cadastra um tipo de usuário 
        /// </summary>
        /// <returns>Insere no banco um tipo de usuário</returns>
        [HttpPost]
        public async Task<ActionResult<Tipo>> Post(Tipo tipo){

            try
            {
                await _repositorio.Salvar(tipo);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }

            return tipo;
        }
        /// <summary>
        /// Atualiza o tipo de usuário que já está cadastrado
        /// </summary>
        /// <returns>Atualiza no banco o tipo de usuário</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Tipo tipo){
            // Se o id do objeto não existir, ele retorna erro 400
            if(id != tipo.IdTipo){
                return BadRequest();
            }
            
            

            try
            {

                await _repositorio.Alterar(tipo);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificamos se o objeto inserido realmente existe no banco
                var tipo_valido = await _repositorio.BuscarPorId(id);

                if(tipo_valido == null){
                    return NotFound();
                }else{

                throw;
                }

                
            }
            // NoContent = retorna 204, sem nada
            return NoContent();
        }

        // DELETE api/tipo/id
        /// <summary>
        /// Deleta o tipo de usuário já cadastrado no banco 
        /// </summary>
        /// <returns>Atualiza o banco</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tipo>> Delete(int id){
            var tipo = await _repositorio.BuscarPorId(id);
            if(tipo == null){
                return NotFound(new {mensagem = "Não foi possível deletar, pois nenhum tipo foi encontrado para o ID informado, verifique e tente novamente!"});
            }
            await _repositorio.Excluir(tipo);
            
            return tipo;
        }
    }
}