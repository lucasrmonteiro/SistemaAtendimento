using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class StatusNota
    {
        #region Propriedades

        public virtual int Codigo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual int Grupo { get; set; }

        #endregion
    }
}