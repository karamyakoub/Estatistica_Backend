using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public class UsuarioGetResponse()
    {
        public Guid Id{get;set;}
        public string? UserName{get;set;}
        public string? NormalizedUserName{get;set;}
        public string? Email{get;set;}
        public string? NormalizedEmail{get;set;}
        public bool EmailConfirmed{get;set;}
        public string? PhoneNumber{get;set;}
        public bool PhoneNumberConfirmed{get;set;}
        public bool Status { get; set; }
        public string? Role { get; set; }
    }
}
