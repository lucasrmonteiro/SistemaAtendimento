using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace SGCA.Models.Enums
{
    public enum EnumStatus
    {
        PENDENTE = 1,
        ATIVO = 2,
        INATIVO = 3
    }

    public enum EnumStatusTicket
    {
        ABERTO = 1,
        EM_ATENDIMENTO,
        SALVO,
        ENCERRADO
    }

    public enum EnumStatusNota
    {
        [Description("Aberto")]
        ABERTO = 1,
        [Description("Em Andamento")]
        EM_ANDAMENTO,
        [Description("Finalizado com Área")]
        FINALIZADO_COM_AREA,
        [Description("Finalizado")]
        FINALIZADO,
        [Description("Cancelado")]
        CANCELADO,
        [Description("Tratado")]
        TRATADO
    }
}