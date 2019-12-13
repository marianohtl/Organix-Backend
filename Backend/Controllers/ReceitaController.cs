using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Repositories;
using Backend.Domains;
using Backend.Repositories;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    // Definimos nossa rota do controller e dizemos que é um controller de API
    // [Authorize(Roles="1,3")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitaController : ControllerBase
    {
        // GufosContext _contexto = new GufosContext();

        ReceitaRepository _repositorio = new ReceitaRepository();
        UploadRepository _upRepositorio = new UploadRepository();
        

        // GET : api/Receita
          // GET : api/Receita
        /// <summary>
        /// Consulta uma lista de receita cadastradas
        /// </summary>
        /// <returns>Retorna uma lista de receitas</returns>
        [HttpGet]
        public async Task<ActionResult<List<Receita>>> Get(){

            var receitas = await _repositorio.Listar();

            if(receitas == null){
                    return NotFound(new {mensagem = "Não é possível encontrar esta receita."});  
            }

            return receitas;

        }

        // GET : api/Receita2
             // GET : api/Receita2
        /// <summary>
        /// Consulta receita baseado no ID
        /// </summary>
        /// <returns>Retorna uma receita</returns>
   
        [HttpGet("{id}")]
        public async Task<ActionResult<Receita>> Get(int id){

            // FindAsync = procura algo específico no banco
            var receita = await _repositorio.BuscarPorId(id);

            if(receita == null){
                    return NotFound(new {mensagem = "Não é possível encontrar esta receita."}); 
            }

            return receita;

        }

    /// <summary>
    /// Consulta receita baseado por palavra ou letra
    /// </summary>
    /// <returns>Retorna a receita encontrada</returns>    

    [HttpGet ("FiltrarPorNome")]
        public async Task<ActionResult<List<Receita>>> GetFiltro (FiltroViewModel filtro) {

                using (OrganixContext _context = new OrganixContext ()){

                List<Receita> receita = await _context.Receita.Where(c=> c.NomeReceita.Contains(filtro.Filtro)).ToListAsync();
                return receita;
                }
        }

        // POST api/Receita
        /// <summary>
        /// Cadastra uma nova receita no banco
        /// </summary>
        /// <returns>Receita cadastrada</returns>
        [HttpPost]
        public async Task<ActionResult<Receita>> Post([FromForm]Receita receita){
             var arquivo = Request.Form.Files[0];
             receita.Imagem = _upRepositorio.Upload(arquivo,"Resources/Images");
            
                //PROBLEMA AQUI 
                // receita.IdUsuario  = ValidarUser();
                await _repositorio.Salvar(receita);
            
                return receita;
        }

        /// <summary>
        /// Altera uma receita
        /// </summary>
        /// <returns>Envia para o banco</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm]Receita receita){
            // Se o id do objeto não existir, ele retorna erro 400

            if(id != receita.IdReceita){
                return NotFound(new {mensagem = "Receita inexistente."});  
               
            }
            try
            {
                var arquivo = Request.Form.Files[0];
                receita.Imagem = _upRepositorio.Upload(arquivo,"Resources/Images");
                await _repositorio.Alterar(receita);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificamos se o objeto inserido realmente existe no banco
                var receita_valido = await _repositorio.BuscarPorId(id);

                if(receita_valido == null){
                    return NotFound(new {mensagem = "Não é possível alterar este produto."});  
                }else{

                throw;
                } 
            }
            // NoContent = retorna 204, sem nada
            return NoContent();
        }

        // DELETE api/receita/id
         // DELETE api/receita/id
        /// <summary>
        /// Deleta receita
        /// </summary>
        /// <returns>Atualiza o banco</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Receita>> Delete(int id){
            var receita = await _repositorio.BuscarPorId(id);
            if(receita == null){
                return NotFound(new {mensagem = "Receita inexistente."});  
            }
            await _repositorio.Excluir(receita);
            
            return receita;
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