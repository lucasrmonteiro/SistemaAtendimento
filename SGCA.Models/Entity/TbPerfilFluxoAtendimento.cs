using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbPerfilFluxoAtendimento {
        public virtual int CodigoFluxoAtendimento { get; set; }
        public virtual int CodigoPerfil { get; set; }
        public virtual FluxoAtendimento TbFluxoAtendimento { get; set; }
        public virtual Perfil TbPerfil { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as TbPerfilFluxoAtendimento;
			if (t == null) return false;
			if (CodigoFluxoAtendimento == t.CodigoFluxoAtendimento
			 && CodigoPerfil == t.CodigoPerfil)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ CodigoFluxoAtendimento.GetHashCode();
			hash = (hash * 397) ^ CodigoPerfil.GetHashCode();

			return hash;
        }
        #endregion
    }
}
