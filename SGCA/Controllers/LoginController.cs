using SGCA.Controllers.Base;
using SGCA.Models.Entity;
using SGCA.Models.Filters;
using SGCA.Models.Manager;
using SGCA.Models.Util;

using NetUtil.Util.Spring;

using CaptchaMvc.HtmlHelpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SGCA.Models.Helpers;
using SGCA.Models.Enums;
using Renci.SshNet;
using System.IO;
using SGCA.Models.DTO;

namespace SGCA.Controllers
{ 
    [HandleError]
    public class LoginController : UsuarioPrimeiroAcessoController
    {

        #region Métodos relacionados às Views

        /// <summary>
        ///     Método que retorna a View padrão de Login.
        ///     Caso haja algum erro, retorna a View padrão de Erro.
        /// </summary>
        /// <returns>
        ///     Fluxo normal: View padrão de Login.
        ///     Exceção: View padrão de Erro.
        /// </returns>
         public ActionResult Login()
        {
            try
            {
              return View("Login");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }

        }

        /// <summary>
        /// Método que retorna a página Primeiro Acesso
        /// </summary>
        /// <returns></returns>
        public ActionResult PrimeiroAcesso()
        {
            //ViewBag.BagEmpresas = EmpresaManager.FindAll<Empresa>();
            ViewBag.BagPerfis = _perfilManager.FindAll();
            ViewBag.ControllerAction = "Login";
            ViewBag.UrlRequestAction = "PrimeiroAcesso";
            ViewBag.UrlRequestController = "Login";

            return View("PrimeiroAcesso");
        }

        /// <summary>
        /// Método para inserir o usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsereUsuario(Usuario usuario)
        {
            //ViewBag.BagEmpresas = EmpresaManager.FindAll<Empresa>();
            //Adiciona o Perfil Solicitando ao usuario(Perfil Padrão)
            ViewBag.BagPerfis = _perfilManager.FindAll();
            InsereUsuarioNaBase(usuario);
            ViewBag.UrlRequestAction = "PrimeiroAcesso";
            ViewBag.UrlRequestController = "Login";

            return View("PrimeiroAcesso");

        }

        /// <summary>
        ///     Método de Logout. 
        ///     Atribui null ao usuário da sessão.
        ///     Apenas aceita acesso por POST.
        ///     Caso haja algum erro, retorna a View padrão de Erro.
        /// </summary>
        /// <returns>
        ///     Fluxo normal: View padrão de Login.
        ///     Exceção: View padrão de Erro.
        /// </returns>
        public ActionResult Logout()
        {
            try
            {
                Session[Constantes.SESSAO_DO_USUARIO] = null;
                return View("Login");
            }
            catch (Exception ex)
            {
                return View("Erro", ex);
            }

        }

        /// <summary>
        ///     Método que realiza autenticação do usuário.
        ///     Apenas aceita acesso por POST.
        ///     Em caso de sucesso, coloca o objeto usuário na sessão.
        ///     Caso haja algum erro, retorna a View padrão de Erro.
        /// </summary>
        /// <param name="login">
        ///     Parâmetro do modelo FiltroLogin.
        /// </param>
        /// <returns>
        ///     Fluxo normal:
        ///         -   Em caso de sucesso redireciona para a Index da Home.
        ///         -   Em caso de falha retorna para a View padrão de Login,
        ///             com os erros.
        ///     Exceção: View padrão de Erro.
        /// </returns>
        [HttpPost]
        public ActionResult Autentica(FiltroLogin login)
        {
            Usuario usuario = null;      
                                                                  
            try
            {
                if (ModelState.IsValid)
                {
                    //if (this.IsCaptchaValid(""))
                    //{
                        usuario = _usuarioManager.
                                    BuscaUsuarioPorLogin(login.Dsc_login);
                            
                        if (usuario != null)
                        {
                            if (ValidaPrimeiroAcesso(usuario))
                            {
                                //RedirectToAction("AlterarSenha", usuario);
                                return View("AlterarSenha");
                            }
                          if (usuario.FlAtivo ?? true)
                          {

                                if (ValidaSenha(usuario.Dsc_senha, 
                                                login.Dsc_senha))
                                {
                                    usuario.Num_tentativas = 0;
                                    _usuarioManager.SaveOrUpdate(usuario);

                                    PreparaSessao(usuario);                                   

                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    usuario.Num_tentativas += 1;

                                    if (usuario.Num_tentativas >= 3)
                                    {
                                        Status statusInativo = _statusManager.
                                            FindByPk((int)EnumStatus.INATIVO);

                                        usuario.Status = statusInativo;
                                    }

                                    ModelState.AddModelError("", SGCA.Models.Properties.Resources.erro_acesso_invalido);
                                }

                                _usuarioManager.SaveOrUpdate(usuario);

                          }
                          else
                          {
                              ModelState.AddModelError("", SGCA.Models.Properties.Resources.erro_acesso_invalido);
                          }
                        }
                        else
                        {
                            ModelState.AddModelError("", SGCA.Models.Properties.Resources.erro_acesso_invalido);
                        }
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", Resources.
                    //                AplicationMessagesResource.
                    //                        erro_acesso_invalido);

                    //    ModelState.AddModelError("", Resources.
                    //        AplicationMessagesResource.erro_codigo_invalido);
                    //}
                }
                else
                {
                    ModelState.AddModelError("", SGCA.Models.Properties.Resources.erro_acesso_invalido);
                }

                return View("Login", login);
            }
            catch (Exception ex)
            {
               return View("ErrorLogin", ex);
            }
        }

        private void PreparaSessao(Usuario userLogged)
        {
            var applicationUsers = (IDictionary<string, HttpSessionStateBase>)HttpContext.Application[Constantes.SESSOES_DOS_USUARIOS];
            if (applicationUsers.ContainsKey(userLogged.Dsc_login))
            {
                applicationUsers[userLogged.Dsc_login].Abandon();
                applicationUsers.Remove(userLogged.Dsc_login);
            }

            applicationUsers.Add(userLogged.Dsc_login, Session);

            SessaoDoUsuario sessaoDoUsuario = new SessaoDoUsuario(userLogged);
            Session[Constantes.SESSAO_DO_USUARIO] = sessaoDoUsuario;
        }

        /// <summary>
        ///     Método que retorna a View padrão de Alteração de Senha.
        ///     Caso haja algum erro, retorna a View padrão de Erro.
        /// </summary>
        /// <returns>
        ///     Fluxo normal: View padrão de Alteração de Senha.
        ///     Exceção: View padrão de Erro.
        /// </returns>        
        public ActionResult AlterarSenha()
        {
            try
            {
                // Atributo colocado na ViewBag para controle
                // da mensagem de sucesso da operação.
                // Caso "Status" seja false, não exibe mensagem de sucesso.
                ViewBag.Status = false;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        /// <summary>
        ///     Método que realiza alteração de senha do usuário.
        ///     Apenas aceita acesso por POST.
        ///     Caso haja algum erro, retorna a View padrão de Erro.
        /// </summary>
        /// <param name="login">
        ///     Parâmetro do modelo FiltroAlterarSenha.
        /// </param>
        /// <returns>
        ///     Fluxo normal: Retorna para View de alteração de senha.
        ///     Exceção: View padrão de Erro.
        /// </returns>
        [HttpPost]
        public ActionResult AlterarSenha(FiltroAlterarSenha login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (this.IsCaptchaValid(""))
                    //{
                        Usuario usuario = ValidaLoginSenha(
                                        login.Dsc_login, login.Dsc_senha);

                        if (usuario != null)
                        {
                            if ( ValidaNovaSenha(usuario.Dsc_senha, 
                                                login.NovaSenha, 
                                                login.ConfirmaNovaSenha ) )
                            {
                                if (AtualizaSenhaNoBanco(usuario, 
                                                login.NovaSenha) != null)
                                {
                                    // Atributo "Status" com valor true sinaliza
                                    // para a View que a mensagem de sucesso
                                    // deve ser exibida.
                                    ViewBag.Status = true;
                                }
                                return View();
                            }
                            else
                            {
                                ModelState.AddModelError("", SGCA.Models.Properties.Resources.erro_dados_incorretos);
                            }
                        }
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", Resources.
                    //        AplicationMessagesResource.erro_codigo_invalido);
                    //}
                }
                else
                {
                    ModelState.AddModelError("", SGCA.Models.Properties.Resources.erro_todos_campos_obrigatorios);
                }

                return View("AlterarSenha", login);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        /// <summary>
        ///     Método que retorna a View padrão de Esquecimento de Senha.
        ///     Caso haja algum erro, retorna a View padrão de Erro.
        /// </summary>
        /// <returns>
        ///     Fluxo normal: View padrão de Esquecimento Senha.
        ///     Exceção: View padrão de Erro.
        /// </returns>  
        public ActionResult EsqueciSenha()
        {
            try
            {
                // Atributo colocado na ViewBag para controle da mensagem
                // de sucesso da operação.
                // Caso "Status" seja false, não exibe mensagem de sucesso.
                ViewBag.Status = false;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        /// <summary>
        ///     Método que realiza alteração de senha do usuário.
        ///     Aceita apenas acesso por POST.
        ///     Em caso de alteração com sucesso, envia email ao usuário.
        ///     Caso haja algum erro, retorna a View padrão de Erro.
        /// </summary>
        /// <param name="login">
        ///     Parâmetro do modelo FiltroAlterarSenha.
        /// </param>
        /// <returns>
        ///     Fluxo normal: Retorna para View de alteração de senha.
        ///     Exceção: View padrão de Erro.
        /// </returns>
        [HttpPost]
        public ActionResult EsqueciSenha(FiltroEsqueciSenha login)
        {
            try
            {
                string novaSenha;

                if (ModelState.IsValid)
                {
                    //if (this.IsCaptchaValid(""))
                    //{
                        Usuario usuario = ValidaLoginEmail(login.Dsc_login,
                                                            login.Dsc_email);

                        if (usuario != null)
                        {
                            novaSenha = PasswordUtils.GerarSenha();
                            AtualizaSenhaNoBanco(usuario, novaSenha);

                            
                            //Para DEBUG
                            //@ViewBag.novaSenha = novaSenha;

                           EnviarEmail((int)EnumEmailTipo.ESQUECI_SENHA, 
                                                        usuario, novaSenha);

                            // Atributo "Status" com valor true sinaliza
                            // para a View que a mensagem de sucesso
                            // deve ser exibida.
                            @ViewBag.Status = true;

                            return View();
                        }
                        else
                        {
                            ModelState.AddModelError("", SGCA.Models.Properties.Resources.erro_login_email_incorretos);
                        }
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", Resources.
                    //        AplicationMessagesResource.erro_codigo_invalido);
                    //}
                }

                return View("EsqueciSenha", login);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        #endregion


        #region Métodos auxiliares de validação e operação com o banco
        
        /// <summary>
        ///     Utiliza o Manager de usuário para atualizar a senha no banco.
        ///     Computa a Hash SHA256 para a nova senha.
        /// </summary>
        /// <param name="usuario">
        ///     Usuário a ter a senha atualizada.
        /// </param>
        /// <param name="novaSenha">
        ///     String da nova senha a ser computada com SHA256.
        /// </param>
        /// <returns>
        ///    Sucesso: Objeto Usuario.
        ///    Falha: null.
        /// </returns>
        private Usuario AtualizaSenhaNoBanco(Usuario usuario, string novaSenha)
        {
            return _usuarioManager.AtualizaSenha(usuario.Dsc_login, 
                                EncryptionUtils.SHA256Hash(novaSenha));
        }

        /// <summary>
        ///     Método que realiza a validação do usuário no banco
        ///     usando 'login' e 'senha'.
        /// </summary>
        /// <param name="login">
        ///     String para busca do usuário no banco.
        /// </param>
        /// <param name="senha">
        ///     String a ser transformada com SHA256
        ///     e comparada com a senha do banco.
        /// </param>
        /// <returns>
        ///     Em caso de sucesso, retorna uma instância de 'Usuario'.
        ///     Em caso de falha, retorna 'null'.
        /// </returns>
        private Usuario ValidaLoginSenha(string login, string senha)
        {            
            Usuario usuario = _usuarioManager.BuscaUsuarioPorLogin(login);

            if (usuario != null)
            {
                if (usuario.Dsc_senha.Equals(EncryptionUtils.
                                                SHA256Hash(senha)))
                {
                    return usuario;
                }

                ModelState.AddModelError("", SGCA.Models.Properties.Resources.erro_senha_incorreta);
            }
            else
            {
                ModelState.AddModelError("", SGCA.Models.Properties.Resources.mensagem_erro_login_incorreto);
            }
            
            return null;            
        }

        /// <summary>
        ///     Compara se duas senhas são iguais.
        ///     Uma é a senha armazenada no banco já transformada
        ///     por Hash SHA256.
        ///     A outra é a senha antes da transformação pela função de Hash.
        /// </summary>
        /// <param name="senhaBanco">
        ///     String da senha armazenado no banco.
        /// </param>
        /// <param name="senhaCandidata">
        ///     String da senha candidata.
        /// </param>
        /// <returns>
        ///     True, caso a string senhaBanco for igual à senhaCandidata
        ///     após a transformação por SHA256.
        /// </returns>
        private bool ValidaSenha(string senhaBanco, string senhaCandidata)
        {
            return senhaBanco.Equals(EncryptionUtils.SHA256Hash(senhaCandidata));
            //return true;
        }

        /// <summary>
        ///     Valida se o email corresponde ao usuário.
        /// </summary>
        /// <param name="login">
        ///     String com o login do usuário a ser buscado.
        /// </param>
        /// <param name="email">
        ///     String com o email a ser comparado com o do usuário encontrado.
        /// </param>
        /// <returns>
        ///     Sucesso: Objeto Usuario.
        ///     Falha: 'null'.
        /// </returns>
        private Usuario ValidaLoginEmail(string login, string email)
        {
            Usuario usuario = _usuarioManager.BuscaUsuarioPorLogin(login);

            if (usuario != null)
            {
                if (usuario.Dsc_email.Equals(email))
                {
                    return usuario;
                }
            }

            return null;
        }

        /// <summary>
        ///     Valida se a nova senha é válida.
        ///     Se nova senha e confirmação de nova senha forem iguais e
        ///     se forem diferentes da atual, então é válida.
        /// </summary>
        /// <param name="atual">
        ///     String representando a hash da senha atual.
        /// </param>
        /// <param name="nova">
        ///     String da nova senha.
        /// </param>
        /// <param name="confirmacao">
        ///     String da confirmação de nova senha.
        /// </param>
        /// <returns>
        ///     Sucesso: true.
        ///     Falha: false.
        /// </returns>
        private bool ValidaNovaSenha(string atual, string nova, string confirmacao)
        {
            bool eValida = false;

            if (nova.Equals(confirmacao))
            {
                // Como a senha atual está armazenada em forma de hash na base,
                // então é necessário fazer o hash na nova senha
                // para comparação.
                if ( !(EncryptionUtils.SHA256Hash(nova).Equals(atual) ) )
                {
                    eValida = true;
                }
            }

            return eValida;
        }

        private bool ValidaPrimeiroAcesso(Usuario usu)
        {
            bool eValida = false;

            if (usu.primeiro_acesso)
            {
                    eValida = true;
            }

            return eValida;
        }

        #endregion

    }
}
