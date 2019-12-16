using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.Repositories;
using Backend.Repository;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    // Definimos nossa rota do controller e dizemos que é um controller de API
    [Route("api/[controller]")]
    [ApiController]
    public class FiltroController : ControllerBase
    {
        // organixContext _contexto = new organixContext();

        FiltroRepository _repositorio = new FiltroRepository();

        // GET : api/Oferta
        /// <summary>
        /// Filtra as ofertas pelos parâmetros solicitados
        /// </summary>
        /// <returns>Retorna uma lista de ofertas</returns>
        [Authorize(Roles="1,3")]
        [HttpPost]
        public async Task<ActionResult<List<OfertaViewModel>>> Get(FiltroViewModel Dados){
            

            var ofertas = await _repositorio.Filtro(Dados);

            if(ofertas == null){
                return NotFound(new {mensagem = "Nenhuma oferta foi encontrada!"});

            }
            return ofertas;
        }

    }
}

    //   var produto = await _repositorio.BuscarPorId(id);

    //         if(produto == null){
    //             return NotFound();
    //         }
