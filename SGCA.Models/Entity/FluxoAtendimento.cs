using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class FluxoAtendimento
    {
        #region Propriedades

        public virtual int Codigo { get; set; }
        public virtual string DescricaoFluxoAtendimento { get; set; }

        #endregion
    }
}