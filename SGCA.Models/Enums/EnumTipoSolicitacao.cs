using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace SGCA.Models.Enums
{
    public enum EnumTipoSolicitacao
    {
        [Description("ARSESP")]
        ARSESP = 1,
        [Description("RECLAME AQUI")]
        RECLAME_AQUI,
        [Description("OUVIDORIA")]
        OUVIDORIA,
        [Description("PROCON")]
        PROCON,
        [Description("INDUSTRIAL")]
        INDUSTRIAL,
        [Description("LOJA")]
        LOJA,
        [Description("PEQUENO COMERCIO")]
        PEQUENO_COMERCIO,
        [Description("CLIENTE")]
        CLIENTE,
        [Description("CONTRATOS")]
        CONTRATOS,
        [Description("DEMANDA INTERNA")]
        DEMANDA_INTERNA,
        [Description("OUTROS")]
        OUTROS,
    }
}