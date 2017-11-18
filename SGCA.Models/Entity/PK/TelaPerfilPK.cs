using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class TelaPerfilPK
    {
        private int cod_perfil;
        private int cod_tela;

        public virtual int Cod_tela
        {
          get { return cod_tela; }
          set { cod_tela = value; }
        }
        public virtual int Cod_perfil
        {
          get { return cod_perfil; }
          set { cod_perfil = value; }
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            bool validate = false;

            // Validate equals
            if (o != null && o is TelaPerfilPK)
            {
                TelaPerfilPK pk = o as TelaPerfilPK;
                if (this.Cod_perfil == pk.Cod_perfil && this.Cod_tela == pk.Cod_tela)
                {
                    validate = true;
                } // end if
            } // end if

            // Return validate
            return validate;
        }

        /// <summary>
        /// HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.Cod_tela + "|" + this.Cod_perfil).GetHashCode();
        } 
    }

    
}