using System;
using System.Text;
using System.Collections.Generic;


namespace SGCA.Models.Entity
{
    
    public class TbUsuarioPerfil {
        public virtual int CodigoUsuario { get; set; }
        public virtual int CodigoPerfil { get; set; }
        public virtual Usuario TbUsuario { get; set; }
        public virtual Perfil TbPerfil { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as TbUsuarioPerfil;
			if (t == null) return false;
			if (CodigoUsuario == t.CodigoUsuario
			 && CodigoPerfil == t.CodigoPerfil)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ CodigoUsuario.GetHashCode();
			hash = (hash * 397) ^ CodigoPerfil.GetHashCode();

			return hash;
        }
        #endregion
    }
}
