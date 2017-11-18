using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SGCA.Models.Validators;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class Perfil
    {

        private int? id_perfil;
        private String dsc_descricao;
        //private IList<Usuario> usuarios;
        //private IList<Funcao> funcoes;

        public Perfil(String descricao,int id)
        {
            Dsc_descricao = descricao;
            Id_perfil = id;
        }

        public Perfil()
        {
        }

        #region Getters e Setters

        [Required(ErrorMessage = "Required.")]
        public virtual int? Id_perfil
        {
            get { return id_perfil; }
            set { id_perfil = value; }
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

        //public virtual IList<Funcao> Funcoes
        //{
        //    get { return funcoes; }
        //    set { funcoes = value; }
        //}

        #endregion

        #region Equals And HashCode Overrides
        /// <summary> 
        /// local implementation of Equals based on unique value members 
        /// </summary> 
        //public override bool Equals(object obj)
        //{
        //    if (this == obj) return true;
        //    if (obj == null) return false;
        //    Perfil castObj = (Perfil)obj;
        //    return (castObj != null) && (this.Id_perfil == castObj.Id_perfil);
        //}

        /// <summary> 
        /// local implementation of GetHashCode based on unique value members 
        /// </summary> 
        public override int GetHashCode()
        {
            int hash = 57;
            hash = 27 * hash * Id_perfil.GetHashCode();
            return hash;
        }
        #endregion
    }
}