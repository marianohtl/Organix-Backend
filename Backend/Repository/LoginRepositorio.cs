using System.Linq;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class LoginRepositorio
    {
          OrganixContext _context = new OrganixContext();
        public Usuario AutenticarUsuario(LoginViewModel login)
        {
            using(OrganixContext _contexto = new OrganixContext()){
                var usuario =   _context.Usuario.Include( l => l.IdTipoNavigation).FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);
                return usuario;
            }
        }
        


    }
}