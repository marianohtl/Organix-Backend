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
    public class EnderecoController : ControllerBase
    {
        // organixContext _contexto = new organixContext();

        EnderecoRepository _repositorio = new EnderecoRepository();


        // GET : api/Endereco
        /// <summary>
        /// Lista os enderecos cadastrados no banco
        /// </summary>
        /// <returns>Retorna uma lista de endereços</returns>
        [HttpGet]
        public async Task<ActionResult<List<Endereco>>> Get(){

            var enderecos = await _repositorio.Listar();

            if(enderecos == null){
                return NotFound(new {mensagem = "Nenhum endereço foi encontrado!"});
            }

            return enderecos;

        }

        // GET : api/Endereco2
        /// <summary>
        /// Consulta um endereço no banco pelo ID
        /// </summary>
        /// <returns>Retorna um endereço cadastrado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Endereco>> Get(int id){

            // FindAsync = procura algo específico no banco
            var endereco = await _repositorio.BuscarPorId(id);
            
            
            if(endereco == null){
                return NotFound(new {mensagem = "Endereço inválido"});
            }


            return endereco;

        }

        // POST api/Endereco
        /// <summary>
        ///  Cria um novo endereço
        /// </summary>
        /// <returns>Enviar para o banco o endereço cadastrado</returns>
        [HttpPost]
        public async Task<ActionResult<Endereco>> Post(Endereco endereco){

            try
            {
                await _repositorio.Salvar(endereco);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }

            return endereco;
        }
        /// <summary>
        /// Altera o endereço solicitado pelo usuário baseado pelo ID
        /// </summary>
        /// <returns>Altera o endereço no banco </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Endereco endereco){
            // Se o id do objeto não existir, ele retorna erro 400
            if(id != endereco.IdEndereco){
                return BadRequest(new {mensagem = "Nenhum endereço encontrado!"});
            }
            
            try
            {

                await _repositorio.Alterar(endereco);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificamos se o objeto inserido realmente existe no banco
                var endereco_valido = await _repositorio.BuscarPorId(id);

                if(endereco_valido == null){
                    return NotFound(new {mensagem = "Nenhum endereço foi encontrado"});
                }else{

                throw;
                }

                
            }
            // NoContent = retorna 204, sem nada
            return NoContent();
        }

        // DELETE api/endereco/id
        /// <summary>
        /// Deleta um endereço baseado no ID
        /// </summary>
        /// <returns>Deleta o endereço cadastrado</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Endereco>> Delete(int id){
            var endereco = await _repositorio.BuscarPorId(id);
            if(endereco == null){
                return NotFound(new {mensagem = "Nenhum endereço foi encontrado"});
            }
            await _repositorio.Excluir(endereco);
            
            return endereco;
        }
    }
}