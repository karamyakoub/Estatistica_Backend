using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Enums
{
    public enum PlanilhaStatusEnum
    {
        AguardandoInclusao = 0,
        Incluida = 1,
        AguardandoProcessamento = 2,
        Pendente = 3,
        EmProcesso = 4,
        ProdutosIncluidos = 5,
        CabecalhosIncluidos = 6,
        ItensIncluidos = 7,
        ProcessamentoConcluido = 8,
        Erro = 9,
    }
}
