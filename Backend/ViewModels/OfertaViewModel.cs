using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
    public class OfertaViewModel
    {
        [Key]
        public int id_oferta { get; set; }
        public string regiao {get;set;}
        public decimal preco {get;set;}
        public DateTime data_fabricacao {get;set;}
        public DateTime data_vencimento {get;set;}
        public string estado_produto {get;set;}
        public string nome_produto  {get;set;}
        public string imagem  {get;set;}
        public string telefone  {get;set;}
        public string celular  {get;set;}
        public string rua  {get;set;}
        public string bairro  {get;set;}
        public string estado  {get;set;}
        public string cidade  {get;set;}
        public string CEP  {get;set;}
        public string nome {get;set;}
        public int id_usuario {get;set;}



    }
}