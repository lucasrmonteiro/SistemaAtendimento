using SGCA.Controllers.Base;
using System.Web.Mvc;
using SGCA.Models.Filters;

namespace SGCA.Controllers
{    
    public class HomeController : BaseController
    {
        /// <summary>
        ///     Método que retorna a View padrão da Home.
        ///     Aplica o filtro 'FiltroAutorizacao'.
        /// </summary>
        /// <returns>
        ///     Fluxo normal: View padrão de Login.
        ///     Falha do filtro: View redirecionada pelo filtro.
        /// </returns>
        [FiltroAutorizacao]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Controller sem especificação!!!
        /// </summary>
        /// <returns></returns>
        [FiltroAutorizacao]
        public ActionResult ErroAcesso()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [FiltroAutorizacao]
        public ActionResult ErrorGeneric()
        {
          return View("Error");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Error404()
        {
            return View("ErrorGeneric");
        }

    }
}
