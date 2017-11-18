using SGCA.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class Funcao
    {
        private int _idFuncao;
        private string _controller;
        private string _action;
        
        public virtual int IdFuncao
        {
            get { return _idFuncao; }
            set { _idFuncao = value; }
        }

        public virtual string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public virtual string Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        public override string ToString()
        {
            return Funcao.ToString(_controller, _action);
        }

        public static string ToString(string controller, string action)
        {
            return new StringBuilder().Append(controller)
                                      .Append(Constantes.SEPARADOR)
                                      .Append(action).ToString();
        }

        #region Equals And HashCode Overrides

        /// <summary> 
        /// local implementation of Equals based on unique value members 
        /// </summary> 
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if ((obj == null) || (obj.GetType() != this.GetType())) return false;
            Funcao castObj = (Funcao)obj;
            return (castObj != null) && (this.IdFuncao == castObj.IdFuncao);
        }

        /// <summary> 
        /// local implementation of GetHashCode based on unique value members 
        /// </summary> 
        public override int GetHashCode()
        {
            int hash = 57;
            hash = 27 * hash * this.IdFuncao.GetHashCode();
            return hash;
        }

        #endregion
    }
}
