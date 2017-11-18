using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SGCA.Models.Validators;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class Usuario
    {
        /// <summary>
        /// Id do usuario
        /// </summary>
        private int id_usuario;

        /// <summary>
        /// Nome do Usuario
        /// </summary>
        private String dsc_nome;

        /// <summary>
        /// Cpf do Usuario
        /// </summary>
        private String dsc_cpf;

        /// <summary>
        /// Email do Usuario
        /// </summary>
        private String dsc_email;

        /// <summary>
        /// Telefone do Usuario
        /// </summary>
        private String dsc_telefone;

        /// <summary>
        /// Descrição da Area
        /// </summary>
        private String dsc_area;

        /// <summary>
        /// Empresa do Usuario
        /// </summary>
        //private Empresa empresa;

        /// <summary>
        /// Status do Usuario
        /// </summary>
        private Status status;

        /// <summary>
        /// Perfil do Usuario
        /// </summary>
        private Perfil perfil;

        /// <summary>
        /// Login do Usuario
        /// </summary>
        private String dsc_login;

        /// <summary>
        /// Senha do Usuario
        /// </summary>
        private String dsc_senha;

        /// <summary>
        /// Numero de Tentativas de Acesso
        /// </summary>
        private int num_tentativas;

        private List<Perfil> perfils;
        
        /// <summary>
        /// Construtor
        /// </summary>
        public Usuario()
        {
        }

        #region Getters e Setters

        /// <summary>
        /// Id do usuario
        /// </summary>
        public virtual int Id_usuario
        {
            get { return id_usuario; }
            set { id_usuario = value; }
        }

        /// <summary>
        /// Nome do Usuario
        /// Validações:
        ///  - Required
        ///  - StringLength (100)
        /// </summary>
        [Required(ErrorMessage="O Campo Nome é obrigatório.")]
        [StringLength(100,ErrorMessage="O campo Nome excedeu o tamanho limite de caracteres!")]
        public virtual String Dsc_nome
        {
            get { return dsc_nome; }
            set { dsc_nome = value; }
        }

        /// <summary>
        /// CPF do Usuario
        /// Validações:
        ///  - Required
        ///  - StringLength
        ///  - CpfValidator
        /// </summary>
        //[Required(ErrorMessage = "O Campo Cpf é obrigatório.")] 
        //[StringLength(14, ErrorMessage = "O campo Cpf excedeu o tamanho limite de caracteres!")]
        //[CpfValidator(ErrorMessage="CPF Inválido")]
        public virtual String Dsc_cpf
        {
            get { return dsc_cpf; }
            set { dsc_cpf = value; }
        }

        /// <summary>
        /// Email do Usuario
        /// Validações:
        ///  - Required
        ///  - StringLength
        ///  - EmailAddress
        /// </summary>
        [Required(ErrorMessage = "O Campo Email é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Email excedeu o tamanho limite de caracteres!")]
        [EmailAddress(ErrorMessage = "Email Inválido")]
        public virtual String Dsc_email
        {
            get { return dsc_email; }
            set { dsc_email = value; }
        }

        /// <summary>
        /// Telefone do Usuario
        /// Validações:
        ///  - Required
        ///  - StringLength
        /// </summary>
        [Required(ErrorMessage = "O Campo telefone é obrigatório.")]
        [StringLength(16, ErrorMessage = "O campo Telefone excedeu o tamanho limite de caracteres!")]
        public virtual String Dsc_telefone
        {
            get { return dsc_telefone; }
            set { dsc_telefone = value; }
        }

        /// <summary>
        /// Area do Usuario
        /// Validações:
        ///  - Required
        ///  - StringLength
        /// </summary>
        //[Required(ErrorMessage = "O Campo Area é obrigatório.")]
        //[StringLength(100, ErrorMessage = "O campo Area excedeu o tamanho limite de caracteres!")]
        public virtual String Dsc_area
        {
            get { return dsc_area; }
            set { dsc_area = value; }
        }

        /// <summary>
        /// Empresa do Usuario
        /// </summary>
        //public virtual Empresa Empresa
        //{
        //    get { return empresa; }
        //    set { empresa = value; }
        // }

        /// <summary>
        /// Perfil do Usuario
        /// </summary>
        public virtual Perfil Perfil
        {
            get { return perfil; }
            set { perfil = value; }
        }

        public virtual List<Perfil> Perfils
        {
            get { return perfils; }
            set { perfils = value; }
        }

        /// <summary>
        /// Status do Usuario
        /// </summary>
        public virtual Status Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// Login do Usuario
        /// </summary>
        [Required(ErrorMessage = "O Campo Login é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Area excedeu o tamanho limite de caracteres!")]
        [LoginValidator(ErrorMessage = "Login Existente")]
        public virtual String Dsc_login
        {
            get { return dsc_login; }
            set { dsc_login = value; }
        }

        /// <summary>
        /// Senha do Usuario
        /// </summary>
        public virtual String Dsc_senha
        {
            get { return dsc_senha; }
            set { dsc_senha = value; }
        }

        /// <summary>
        /// Numero de tentativas de acesso
        /// </summary>
        public virtual int Num_tentativas
        {
            get { return num_tentativas; }
            set { num_tentativas = value; }
        }

        public virtual IList<Grupo> Grupos { get; set; }


        public virtual bool primeiro_acesso { get; set; }

        public virtual bool? FlAtivo { get; set; }

        #endregion

        #region Equals And HashCode Overrides
        /// <summary> 
        /// local implementation of Equals based on unique value members 
        /// </summary> 
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if ((obj == null) || (obj.GetType() != this.GetType())) return false;
            Usuario castObj = (Usuario)obj;
            return (castObj != null) && (this.Id_usuario == castObj.Id_usuario);
        }

        /// <summary> 
        /// local implementation of GetHashCode based on unique value members 
        /// </summary> 
        public override int GetHashCode()
        {
            int hash = 57;
            hash = 27 * hash * Id_usuario.GetHashCode();
            return hash;
        }
        #endregion
    }
}