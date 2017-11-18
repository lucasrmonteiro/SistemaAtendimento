using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGCA.Models.Helpers;
using SGCA.Models.Entity;
using SGCA.Models.Enums;
using SGCA.Models.Manager;
using SGCA.Models.Validators;
using NetUtil.Util.Spring;
using Resources;
using SGCA.Models.Util;

namespace SGCA.Controllers.Base
{
    public class UsuarioPrimeiroAcessoController : BaseController
    {
        #region Managers
        protected IUsuarioManager _usuarioManager = ServiceLocator.GetObject<IUsuarioManager>();

        protected IStatusManager _statusManager = ServiceLocator.GetObject<IStatusManager>();

        protected IPerfilManager _perfilManager = ServiceLocator.GetObject<IPerfilManager>();

        protected IApplicationManager _appManager = ServiceLocator.GetObject<IApplicationManager>();
        #endregion Managers

        /// <summary>
        /// Método que faz a lógica de inserção do usuario na base
        /// </summary>
        /// <param name="usuario"></param>
        public void InsereUsuarioNaBase(Usuario usuario)
        {
            //Adiciona o Perfil Solicitando ao usuario(Perfil Padrão)
            //usuario.Perfil = _perfilManager.FindByPk((int)EnumPerfil.ADMINISTRADOR);
            //Adiciona o Status pendente ao usuario
            usuario.Status = _statusManager.FindByPk((int)EnumStatus.PENDENTE);
            //Validador de Campos Obrigatórios,Tamanho dos Campos,CPF e Email          
            //if (ModelState.IsValid & !ValidaCamposObrigatorios(usuario))
            if (!ValidaCamposObrigatorios(usuario))
            {
                //Insere a instancia de perfil no objeto usuario
                //usuario.Perfil = PerfilManager.FindByPk<Perfil>(usuario.Perfil.Id_perfil);
                //Insere o usuário na base
                var novaSenha = PasswordUtils.GerarSenha();
                usuario.Dsc_senha = EncryptionUtils.SHA256Hash(novaSenha);
                usuario.primeiro_acesso = true;
                usuario.FlAtivo = true;
                _usuarioManager.InsereUsuarioNaBase(usuario);
                //Envia email para os administradores
                //EnviarEmail((int)EnumEmailTipo.PRIMEIRO_ACESSO, usuario, null);
                
                EnviarEmail((int)EnumEmailTipo.ACESSO_LIBERADO, usuario, novaSenha);

                //Mensagem de sucesso
                ViewBag.Status_Cadastro = true;
            }
        }

        /// <summary>
        /// Método que valida os campos obrigatórios
        /// </summary>
        /// <param name="solicitacao"></param>
        public bool ValidaCamposObrigatorios(Usuario usuario)
        {
            bool possuiErro = false;
            try
            {
                // Valida Classificacao
                //if (null == usuario.Empresa || usuario.Empresa.Id_empresa == 0)
                //{
                //    ModelState.AddModelError("usuario.Empresa", ViewMessagesResource.commom_obrigatorio_empresa);
                //    possuiErro = true;
                //}

                // Valida Produto
                if (null == usuario.Perfil || usuario.Perfil.Id_perfil == 0)
                {
                    ModelState.AddModelError("usuario.Perfil", ViewMessagesResource.commom_obrigatorio_perfil);
                    possuiErro = true;
                }
                // Valida Tipo da Solicitação
                if (null == usuario.Status || usuario.Status.Id_status == 0)
                {
                    ModelState.AddModelError("usuario.Status", ViewMessagesResource.commom_obrigatorio_status);
                    possuiErro = true;
                }
                //if (usuario.Dsc_cpf != null)
                //{
                //    // Valida Se cpf existe na base
                //    if (!_usuarioManager.VerificaSeCpfExisteNaBase(CpfUtil.LimpaCarateresCpf(usuario.Dsc_cpf)))
                //    {
                //        ModelState.AddModelError("usuario.Cpf", ViewMessagesResource.common_error_cpf_cadastrado);
                //        possuiErro = true;
                //    }
                //}

                return possuiErro;
            }
            catch (Exception ex)
            {
                View("Error", ex);
            }

            return possuiErro;
        }

        /// <summary>
        /// Método que carrega as combos da tela
        /// </summary>
        /// <param name="usuario"></param>
        public void CarregaCombos(Usuario usuario)
        {
            //Adicicona a lista de perfis na visao
            ViewBag.BagPerfis = _perfilManager.FindAll();
            //Adiciona a lista de statuis na visão
            ViewBag.BagStatus = _statusManager.FindAll();
            //Adicuina a lista de Solicitações na visão
        }

        /// <summary>
        /// Método que faz as validações necessárias para a alteração do usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool ValidaAlteracao(Usuario usuario)
        {
            bool possuiErro = false;
            Usuario baseUsuario = _usuarioManager.BuscaUsuarioPorLogin(usuario.Dsc_login);
            // Valida Cpf
            if (!ModelState.IsValidField("Dsc_cpf"))
            {
                possuiErro = true;
            }

            // Valida Nome
            if (!ModelState.IsValidField("Dsc_nome"))
            {
                possuiErro = true;
            }

            // Valida Nome
            if (!ModelState.IsValidField("Dsc_email"))
            {
                possuiErro = true;
            }

            // Valida Login
            if (!String.IsNullOrEmpty(usuario.Dsc_login))
            {
                if (usuario.Dsc_login.Length > 100)
                {
                    ModelState.AddModelError("usuario.Login", ViewMessagesResource.common_error_login_tamanho_limite);
                    possuiErro = true;
                }
            }

            // Valida Login
            if (String.IsNullOrEmpty(usuario.Dsc_login))
            {
                ModelState.AddModelError("usuario.Login", ViewMessagesResource.commom_obrigatorio_login);
                possuiErro = true;
            }

            // Valida Login
            if (baseUsuario != null)
            {
                if (!usuario.Equals(baseUsuario))
                {
                    ModelState.AddModelError("usuario.Login", ViewMessagesResource.common_error_login_existente);
                    possuiErro = true;
                }
                
            }

            // Valida Area
            //if (!ModelState.IsValidField("Dsc_area"))
            //{
            //    possuiErro = true;
            //}

            // Valida Empresa
            //if (null == usuario.Empresa || usuario.Empresa.Id_empresa == 0)
            //{
            //    ModelState.AddModelError("usuario.Empresa", ViewMessagesResource.commom_obrigatorio_empresa);
            //    possuiErro = true;
            //}

            // Valida Perfil
            if (null == usuario.Perfil || usuario.Perfil.Id_perfil == 0)
            {
                ModelState.AddModelError("usuario.Perfil", ViewMessagesResource.commom_obrigatorio_perfil);
                possuiErro = true;
            }

            // Valida Status
            if (null == usuario.Status || usuario.Status.Id_status == 0)
            {
                ModelState.AddModelError("usuario.Status", ViewMessagesResource.commom_obrigatorio_status);
                possuiErro = true;
            }
            return possuiErro;
        }

        /// <summary>
        /// Método responsável em preparar o email
        /// </summary>
        /// <param name="tipoEmail">Informa o tipo de email</param>
        public void EnviarEmail(int tipoEmail,Usuario usuario,string senha)
        {

            ConfigEmail configemail = _appManager.FindConfigEmail();

            IList<string> listDestinatarios = new List<string>();
            if(tipoEmail.Equals((int)EnumEmailTipo.PRIMEIRO_ACESSO))
            {
                listDestinatarios = CarregaListaEmailAdministradores();
            }

            if (tipoEmail.Equals((int)EnumEmailTipo.ACESSO_LIBERADO) || tipoEmail.Equals((int)EnumEmailTipo.ESQUECI_SENHA))
            {
                listDestinatarios.Add(usuario.Dsc_email);
            }


            //PARA TESTE
            //listDestinatarios.Add(usuario.Dsc_email);
            //listDestinatarios.Add("desenvolvedorinfobase@gmail.com");


            // Carrega o assunto do email
            string titulo = CarregaAssuntoEmail(tipoEmail);

            // Carrega o conteudo do email
            string mensagem = CarregaMensagemEmail(tipoEmail,usuario,senha);

            // Envia o email
            EmailHelper.EnviaEmail(configemail, listDestinatarios, titulo, mensagem);
        }

        /// <summary>
        /// Método que carrega a lista dos destinatários de emails
        /// </summary>
        /// <returns></returns>
        private IList<string> CarregaListaEmailAdministradores()
        {
            return _usuarioManager.GetListaDeEmailDosAdministradores(); ;
        }

        /// <summary>
        /// Método responsável em carregar a mensagem do email
        /// </summary>
        /// <param name="tipoEmail">Tipo do email que será enviado. 3 primeiro acesso 4 liberação do usuario para acessar o sistema</param>
        /// <returns></returns>
        private string CarregaMensagemEmail(int tipoEmail, Usuario usuario,string senha)
        {
            string mensagem = "";

            switch (tipoEmail)
            {
                case (int)EnumEmailTipo.PRIMEIRO_ACESSO:
                    mensagem += "<br><br>" +
                        "<p>Prezado Administrador, existe uma solicitação de acesso ao sistema pendente a aprovação</p>" +
                        //"<p>Empresa: " + "" + "</p>" +
                        "<p>Nome: " + usuario.Dsc_nome + "</p>" +
                        "<p>CPF: " + usuario.Dsc_cpf;
                    break;
                case (int)EnumEmailTipo.ACESSO_LIBERADO:
                case (int)EnumEmailTipo.ESQUECI_SENHA:
                    mensagem += "<br><br>" +
                        "Caro senhor(a) " + usuario.Dsc_nome + ", sua senha para acessar o sistema SGCA é: " + senha +
                        "<br><br> Sistema SGCA.";
                    break;    
            }

            

            return mensagem;
        }
    }
}
