using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public class DashboardDto
    {
        public int TotalUsers {  get; set; }
        public int TotalConcorrentes { get; set; }
        public int TotalProducts { get; set; }
        public int TotalNotas { get; set; }
        public int TotalLinkedProducts { get; set; }
        public int TotalUnLinkedProducts { get; set; }
        public List<DashboardProductsCountByConcorrente> TotalProductsConcorrente { get; set; } = new List<DashboardProductsCountByConcorrente>();
        public List<DashboardNotasPorConcorrente> TotalNotasConcorrente { get; set; } = new List<DashboardNotasPorConcorrente>();
        public List<DashboardNotasPorDate> TotalNotasData { get; set; } = new List<DashboardNotasPorDate>();

        public class DashboardProductsCountByConcorrente
        {
            public string? Concorrente { get; set; }            
            public int Count { get; set; }
        }
        public class DashboardNotasPorConcorrente
        {
            public string? Concorrente { get; set; }
            public int Count { get; set; }
        }

        public class DashboardNotasPorDate
        {
            public DateTime Data { get; set; }
            public int Count { get; set; }
        }
    }
}
