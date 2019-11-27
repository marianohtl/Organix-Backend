using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    //- Definimos nossa rota do controller e dizemos que é um controller de API
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaReceitaController : ControllerBase
    {
        // organixContext _contexto = new organixContext();

        CategoriaReceitaRepository _repositorio = new CategoriaReceitaRepository();

        // GET : api/CategoriaReceita
        /// <summary>
        /// Busca todas as categorias de receitas cadastradas 
        /// </summary>
        /// <returns>Retorna uma lista de categorias de receitas</returns>
        [Authorize(Roles="1, 3")]
        [HttpGet]
        public async Task<ActionResult<List<CategoriaReceita>>> Get(){

            var categoriaReceitas = await _repositorio.Listar();

            if(categoriaReceitas == null){
                 return NotFound(new {mensagem = "Não foi encontrada nenhuma Categoria!"});
            }

            return categoriaReceitas;

        }

        // GET : api/CategoriaReceita2
        /// <summary>
        /// Busca uma categoria pelo ID
        /// </summary>
        /// <returns>Retorna uma categoria válida</returns>
        [Authorize(Roles="1, 3")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaReceita>> Get(int id){

            // FindAsync = procura algo específico no banco
            var categoriaReceita = await _repositorio.BuscarPorId(id);

            if(categoriaReceita == null){
                 return NotFound(new {mensagem = "Nenhuma categoria encontrada para o ID informado, verifique e tente novamente!"});
            }

            return categoriaReceita;

        }

        // POST api/CategoriaReceita
        /// <summary>
        ///Cria uma categoria definida pelo usuário administrador
        /// </summary>
        /// <returns>Envia para o banco a categoria criada</returns>
        [Authorize(Roles="1")]
        [HttpPost]
        public async Task<ActionResult<CategoriaReceita>> Post(CategoriaReceita categoriaReceita){

            try
            {
                await _repositorio.Salvar(categoriaReceita);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }

            return categoriaReceita;
        }
         // PUT api/CategoriaReceita
        /// <summary>
        ///Altera uma categoria definida pelo usuário administrador
        /// </summary>
        /// <returns>Envia para o banco a categoria alterada</returns>
        [Authorize(Roles="1")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CategoriaReceita categoriaReceita){
            // Se o id do objeto não existir, ele retorna erro 400
            if(id != categoriaReceita.IdCategoriaReceita){
                return BadRequest(new{mensagem = "Categoria não existente"});
            }
            
            

            try
            {

                await _repositorio.Alterar(categoriaReceita);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificamos se o objeto inserido realmente existe no banco
                var categoriaReceita_valido = await _repositorio.BuscarPorId(id);

                if(categoriaReceita_valido == null){
                     return NotFound(new {mensagem = "Nenhuma categoria encontrada para o ID informado"});
                }else{

                throw;
                }

                
            }
            // NoContent = retorna 204, sem nada
            return NoContent();
        }

        // DELETE api/categoriaReceita/id
        /// <summary>
        /// Deleta uma categoria de receitas
        /// </summary>
        /// <returns>Deleta do banco uma categoria de receita</returns>
        [Authorize(Roles="1")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaReceita>> Delete(int id){
            var categoriaReceita = await _repositorio.BuscarPorId(id);
            if(categoriaReceita == null){
                 return NotFound(new {mensagem = "Nenhuma categoria encontrada para o ID informado, verifique e tente novamente!"});
            }
            await _repositorio.Excluir(categoriaReceita);
            
            return categoriaReceita;
        }
    }
}