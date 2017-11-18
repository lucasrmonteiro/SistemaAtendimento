using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SGCA.Models.Entity;
using SGCA.Models.Manager;
using NetUtil.Util.Spring;
using SGCA.Models.DTO;
using SGCA.Models.Util;

namespace SGCA.Models.Filters
{
    /// <summary>
    /// Classe responsavel pelo filtro de acesso a aplicacao
    /// </summary>
    public class FiltroAutorizacao : ActionFilterAttribute
    {
        /// <summary>
        /// Sobrescreve o método que intercepta a execução de uma ação 
        /// redirecionando quando necessario
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessaoDoUsuario sessaoDoUsuario;

            if (EstaLogado(filterContext, out sessaoDoUsuario))
            {
                if (!EstaLogadoOutraSessao(filterContext, sessaoDoUsuario))
                {
                    if (PossuiAcesso(filterContext, sessaoDoUsuario))
                    {
                        return;
                    }
                    else
                    {
                        filterContext.Result = NaoPossuiAcesso;
                    }
                }
                else
                {
                    filterContext.Result = LoginSimultaneo;
                }
            }
            else
            {
                filterContext.Result = UsuarioDeslogado;
            }
        }

        /// <summary>
        /// Pega o objeto SessaoDoUsuario do HttpContext, define para o parametro 'out' 'sdu' e retorna se é diferente de null
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="sdu"></param>
        /// <returns></returns>
        private bool EstaLogado(ActionExecutingContext filterContext, out SessaoDoUsuario sdu)
        {
            sdu = (SessaoDoUsuario)filterContext.HttpContext.Session[Constantes.SESSAO_DO_USUARIO];
            return sdu != null;
        }

        /// <summary>
        /// verifica se é login simultanteo
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="applicationUsers"></param>
        /// <param name="sdu"></param>
        /// <returns></returns>
        private bool EstaLogadoOutraSessao(ActionExecutingContext filterContext, SessaoDoUsuario sdu)
        {
            var applicationUsers = (IDictionary<string, HttpSessionStateBase>)filterContext.HttpContext.Application[Constantes.SESSOES_DOS_USUARIOS];
            string currentSessionID = filterContext.HttpContext.Session.SessionID;

            return applicationUsers.ContainsKey(sdu.Login) &&
                  !applicationUsers[sdu.Login].SessionID.Equals(currentSessionID);
        }

        /// <summary>
        /// verifica se o usuario da sessao tem acesso
        /// </summary>
        /// <param name="sdu"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool PossuiAcesso(ActionExecutingContext filterContext, SessaoDoUsuario sdu)
        {
            String path = filterContext.HttpContext.Request.Path;
            var pathSplit = path.Split(Constantes.SEPARADOR);

            string action = pathSplit.Last();
            string controller = pathSplit[pathSplit.Length - 2];

            return
                string.IsNullOrWhiteSpace(action) ||
                Constantes.CONTROLLER_HOME.Equals(controller) ||
                sdu.TemPermissao(controller, action);
        }

        /// <summary>
        /// Redireciona para página de login
        /// </summary>
        private ActionResult UsuarioDeslogado
        {
            get
            {
                return new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = Constantes.CONTROLLER_LOGIN, action = Constantes.ACTION_LOGIN }
                    )
                );
            }
        }

        /// <summary>
        /// Redireciona para tratamento de login simultaneo
        /// </summary>
        private ActionResult LoginSimultaneo
        {
            get
            {
                return new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = Constantes.CONTROLLER_LOGIN, action = Constantes.ACTION_LOGIN_SIMULTANEO }
                    )
                );
            }
        }

        /// <summary>
        /// Redireciona para página de erro de acesso
        /// </summary>
        private ActionResult NaoPossuiAcesso
        {
            get
            {
                return new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = Constantes.CONTROLLER_HOME, action = Constantes.ACTION_ERRO_ACESSO }
                    )
                );
            }
        }

    }
}