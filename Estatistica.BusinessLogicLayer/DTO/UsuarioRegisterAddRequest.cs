using Estatistica.DataAccessLayer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public  class UsuarioRegisterAddRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public required UserProfileEnum UserProfile { get; set; }
    }
}
