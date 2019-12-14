using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    // Definimos nossa rota do controller e dizemos que é um controller de API
    
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaController : ControllerBase
    {
        // organixContext _contexto = new organixContext();

        OfertaRepository _repositorio = new OfertaRepository();
        Oferta oferta = new Oferta();

        // GET : api/Oferta
         // GET : api/Oferta
        /// <summary>
        ///  Busca uma lista de ofertas cadastradas
        /// </summary>
        /// <returns>Retorna uma lista de ofertas</returns>
        // [Authorize(Roles="1,2,3")]
        [HttpGet]
        public async Task<ActionResult<List<Oferta>>> Get(){

            var ofertas = await _repositorio.Listar();

            if(ofertas == null){
                return NotFound(new {mensagem = "Nenhuma Oferta foi encontrada!"});
            }
            return ofertas;
        }

        
        // GET : api/Oferta2
        // GET : api/Oferta2
        /// <summary>
        /// Consulta uma oferta pelo ID
        /// </summary>
        /// <returns>Retorna uma oferta válida</returns>
        // [Authorize(Roles="1,2,3")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Oferta>> Get(int id){

            // FindAsync = procura algo específico no banco
            var oferta = await _repositorio.BuscarPorId(id);

            if(oferta == null){
                return NotFound(new {mensagem = "Nenhuma Oferta foi encontrada para o id informado!"});
            }

            return oferta;

        }

        // POST api/Oferta
        // POST api/Oferta
        ///<summary>
        ///Cria uma nova oferta
        ///</summary>
        ///<returns>Envia para o banco o valor cadastrado</returns>
        [Authorize(Roles="1,2")]
        [HttpPost]
        public async Task<ActionResult<Oferta>> Post(Oferta oferta){

            try
            {
                oferta.IdUsuario = ValidarUser();
                await _repositorio.Salvar(oferta);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }
            return oferta;
        }
           /// <summary>
        /// Altera uma oferta cadastrada baseado no ID
        /// </summary>
        /// <returns>Envia para o banco o valor alterado</returns>
        [Authorize(Roles="1,2")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Oferta oferta){
            // Se o id do objeto não existir, ele retorna erro 400
            if(id != oferta.IdOferta){
                return BadRequest();
            }

            try
            {

                await _repositorio.Alterar(oferta);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificamos se o objeto inserido realmente existe no banco
                var oferta_valido = await _repositorio.BuscarPorId(id);

                if(oferta_valido == null){
                    return NotFound(new {mensagem = "Nenhuma Oferta foi encontrada!"});
                }else{

                throw;
                }
                
            }
            // NoContent = retorna 204, sem nada
            return NoContent();
        }

        // DELETE api/oferta/id
          // DELETE api/oferta/id
        /// <summary>
        /// Deleta uma oferta
        /// </summary>
        /// <returns>Deleta uma oferta cadastrada</returns>
        [Authorize(Roles="1,2")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Oferta>> Delete(int id){
            var oferta = await _repositorio.BuscarPorId(id);
            if(oferta == null){
                return NotFound(new {mensagem = "Não foi possível deletar a oferta pois o ID informado não existe!"});
            }
            await _repositorio.Excluir(oferta);
            
            return oferta;
        }

            private int ValidarUser(){
                var identity = HttpContext.User.Identity as ClaimsIdentity; 
                IEnumerable<Claim> claim = identity.Claims;

                var idClaim = claim.Where(x => x.Type == ClaimTypes.PrimarySid).FirstOrDefault();
                
                var id = Convert.ToInt32((idClaim.Value));

                return id;
            }
    }
}