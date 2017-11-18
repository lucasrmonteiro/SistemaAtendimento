using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    public class ConfigEmail
    {
        private int id_config;
        private String dsc_nome_remetente;
        private String dsc_email_remetente;
        private String dsc_servidor_email;
        private String dsc_usuario_email;
        private String dsc_senha_email;
        private int num_porta_email;
        private String char_ssl;


        #region Getters e Setters

        public virtual int Id_config
        {
            get { return id_config; }
            set { id_config = value; }
        }

        public virtual String Dsc_nome_remetente
        {
            get { return dsc_nome_remetente; }
            set { dsc_nome_remetente = value; }
        }

        public virtual String Dsc_email_remetente
        {
            get { return dsc_email_remetente; }
            set { dsc_email_remetente = value; }
        }

        public virtual String Dsc_servidor_email
        {
            get { return dsc_servidor_email; }
            set { dsc_servidor_email = value; }
        }

        public virtual String Dsc_usuario_email
        {
            get { return dsc_usuario_email; }
            set { dsc_usuario_email = value; }
        }

        public virtual String Dsc_senha_email
        {
            get { return dsc_senha_email; }
            set { dsc_senha_email = value; }
        }

        public virtual int Num_porta_email
        {
            get { return num_porta_email; }
            set { num_porta_email = value; }
        }

        public virtual String Char_ssl
        {
            get { return char_ssl; }
            set { char_ssl = value; }
        }

        #endregion

        #region Equals And HashCode Overrides
        /// <summary> 
        /// local implementation of Equals based on unique value members 
        /// </summary> 
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if ((obj == null) || (obj.GetType() != this.GetType())) return false;
            ConfigEmail castObj = (ConfigEmail)obj;
            return (castObj != null) && (this.id_config == castObj.Id_config);
        }

        /// <summary> 
        /// local implementation of GetHashCode based on unique value members 
        /// </summary> 
        public override int GetHashCode()
        {
            int hash = 57;
            hash = 27 * hash * id_config.GetHashCode();
            return hash;
        }
        #endregion
    }
}