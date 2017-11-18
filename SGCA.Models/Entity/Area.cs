﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class Area
    {
        #region Propriedades

        public virtual int Codigo { get; set; }

        public virtual string IdentificacaoArea { get; set; }

        public virtual string DescricaoArea { get; set; }

        #endregion Propriedades
    }
}