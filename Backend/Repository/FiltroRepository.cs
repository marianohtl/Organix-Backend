using System.Threading.Tasks;
using Backend.Domains;
// using Microsoft.Data.SqlClient;
// using System.Linq;
// using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Backend.Interfaces;
using Microsoft.Data.SqlClient;
using Backend.ViewModels;

namespace Backend.Repository
{
    public class FiltroRepository : IFiltro
    {
          public async Task<List<OfertaViewModel>> Filtro(FiltroViewModel Dados)
        {
           using(OrganixContext _contexto = new OrganixContext()){

            
            var produtoQuery = new SqlParameter("@produto",Dados.produto);
            var regiaoQuery = new SqlParameter("@regiao", Dados.regiao);
            var menorPrecoQuery = new SqlParameter("@menorPreco",Dados.menorPreco);
            var maiorPrecoQuery = new SqlParameter("@maiorPreco",Dados.maiorPreco);

            var lista = await _contexto.OfertaViewModel.FromSqlRaw("SELECT usuario.id_usuario, usuario.nome, endereco.regiao, telefone.telefone, telefone.celular, endereco.rua, endereco.bairro, endereco.cidade, endereco.CEP, endereco.estado, oferta.id_produto, oferta.id_oferta, oferta.preco,oferta.data_fabricacao, oferta.data_vencimento, oferta.estado_produto, produto.nome_produto, produto.imagem FROM oferta INNER JOIN usuario on usuario.id_usuario = oferta.id_usuario INNER JOIN endereco on usuario.id_usuario = endereco.id_usuario INNER JOIN telefone on usuario.id_usuario = telefone.id_usuario INNER JOIN produto on produto.id_produto = oferta.id_oferta WHERE oferta.id_produto= @produto and oferta.preco >= @menorPreco and oferta.preco <= @maiorPreco and endereco.regiao= @regiao",produtoQuery,menorPrecoQuery,maiorPrecoQuery,regiaoQuery).ToListAsync();

            return  lista;
            }
        }
    }
}

    //         return produto;

 // string connectionString = @"Data Source =DESKTOP-9J1BUVT\\SQLEXPRESS; Database=Organix; User Id=sa; Password=132";
            // // using (SqlConnection connection = new SqlConnection(connectionString)){

            //   
            //     // string FiltroQuery = ;

                // SqlCommand command = new SqlCommand(connectionString, FiltroQuery);


            // SqlConnection cnn;
            // 

            // cnn=new SqlConnection(connectionString);

            // cnn.Open();


               // SqlConnection conn = new SqlConnection("Server.DESKTOP-9J1BUVT\\SQLEXPRESS; Database=Organix; User Id=sa; Password=132");
            // conn.Open();
            //   SqlCommand cmd = new SqlCommand("select * From Oferta");
            //   SqlDataReader reader = cmd.ExecuteReader();
              
            // return oferta;


