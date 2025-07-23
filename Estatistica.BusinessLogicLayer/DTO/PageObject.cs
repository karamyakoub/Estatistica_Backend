using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public record PageObject<T>(List<T> Data,int PageSize,int PageCount,int Total);
    
}
