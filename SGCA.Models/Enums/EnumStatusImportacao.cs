using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Enums
{
    public enum EnumStatusImportacao
    {
        AGUARDANDO = 1,
        ARQUIVO_REMOVIDO = 2,
        EM_EXECUCAO = 3,
        SUCESSO = 4,
        FALHA = 5
    }
}