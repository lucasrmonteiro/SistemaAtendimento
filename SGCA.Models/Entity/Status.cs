using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    public class Status
    {
        private int id_status;
        private String dsc_descricao;
        //private IList<Usuario> usuarios;

        public Status(String descricao, int id)
        {
            Id_status = id;
            Dsc_descricao = descricao;
        }

        public Status()
        {
        }

        #region Getters e Setters

        public virtual int Id_status
        {
            get { return id_status; }
            set { id_status = value; }
        }

        public virtual String Dsc_descricao
        {
            get { return dsc_descricao; }
            set { dsc_descricao = value; }
        }

        //public virtual IList<Usuario> Usuarios
        //{
        //    get { return usuarios; }
        //    set { usuarios = value; }
        //}


        #endregion

        #region Equals And HashCode Overrides
        /// <summary> 
        /// local implementation of Equals based on unique value members 
        /// </summary> 
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if ((obj == null) || (obj.GetType() != this.GetType())) return false;
            Status castObj = (Status)obj;
            return (castObj != null) && (this.id_status == castObj.Id_status);
        }

        /// <summary> 
        /// local implementation of GetHashCode based on unique value members 
        /// </summary> 
        public override int GetHashCode()
        {
            int hash = 57;
            hash = 27 * hash * id_status.GetHashCode();
            return hash;
        }
        #endregion
    }
}