using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace SGCA.Models.Enums
{
    public enum EnumPerfil
    {
        ADMINISTRADOR = 1,
        ANALISTA_PRIORIZACAO = 2,
        OPERADOR = 3,
        COORDENADOR = 4,
        ANALISTA_NIVEL1 = 5,
        ANALISTA_NIVEL2 = 6,
        ANALISTA_NIVEL3 = 7,
        COBRANCA = 8
        //[Description("Administrador")]
        //ADMINISTRADOR = 1,
        //[Description("Supervisor")]
        //SUPERVISOR,
        //[Description("Supervisor Pendência")]
        //SUPERVISOR_PENDENCIA,
        //[Description("Analista Priorização")]
        //ANALISTA_PRIORIZACAO,
        //[Description("Analista Pendência")]
        //ANALISTA_PENDENCIA
    }
}