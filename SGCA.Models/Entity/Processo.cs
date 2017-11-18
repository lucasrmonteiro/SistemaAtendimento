using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SGCA.Models.Validators;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class Processo
    {
        public virtual int CodigoProcesso { get; set; }
        public virtual Usuario TbUsuario { get; set; }
        public virtual TbDemandaProcesso TbDemandaProcesso { get; set; }
        public virtual TbTipoSolicitacao TbTipoSolicitacao { get; set; }
        public virtual TbStatusProcesso TbStatusProcesso { get; set; }
        public virtual Area TbAreaProcesso { get; set; }
        public virtual FluxoAtendimento TbFluxoAtendimento { get; set; }
        public virtual int NumeroProcesso { get; set; }
        public virtual string SlaCliente { get; set; }
        public virtual string Observacao { get; set; }
        public virtual DateTime? DataCriacao { get; set; }
        public virtual DateTime? DataExtracao { get; set; }
        public virtual DateTime? DataEncerramento { get; set; }
        public virtual bool AtendimentoOutraArea { get; set; }
        public virtual bool? AtendimentoAutomatico { get; set; }

        #region Equals And HashCode Overrides
        /// <summary> 
        /// local implementation of Equals based on unique value members 
        /// </summary> 
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if ((obj == null) || (obj.GetType() != this.GetType())) return false;
            Processo castObj = (Processo)obj;
            return (castObj != null) && (this.CodigoProcesso == castObj.CodigoProcesso);
        }

        /// <summary> 
        /// local implementation of GetHashCode based on unique value members 
        /// </summary> 
        public override int GetHashCode()
        {
            int hash = 57;
            hash = 27 * hash * CodigoProcesso.GetHashCode();
            return hash;
        }
        #endregion

    }
}
