using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public record RoleGetResponse(string? Id,string? RoleName)
    {
        public RoleGetResponse() : this(default,default)
        {
            
        }
    }
    
}
