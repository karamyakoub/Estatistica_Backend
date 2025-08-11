using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface INfeXmlService
    {
        bool IsValidNfe(string xmlContent);
        Task<dynamic> CheckFilialPendente(string xmlContent);
        Task<bool> ImportNfeXml(string xmlContent);
    }
}
