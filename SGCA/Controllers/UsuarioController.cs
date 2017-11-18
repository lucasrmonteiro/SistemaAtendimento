using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGCA.Controllers.Base;
using SGCA.Models.Validators;
using SGCA.Models.Entity;
using SGCA.Models.Manager;
using NetUtil.Util.Spring;
using System.Collections;
using SGCA.Models.Enums;
using SGCA.Models.Filters;
using Resources;
using SGCA.Models.Util;
using NetUtil.Util.Enums;
using SGCA.Models.DTO;
using NetUtil.Util.Hibernate;
using NHibernate.Linq;
using System.Web.Routing;

namespace SGCA.Controllers
{
    [FiltroAutorizacao]
    public class UsuarioController : UsuarioPrimeiroAcessoController
    {

        /// <summary>
        /// Método que retorna a página de cadastro de usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult CadastroUsuario()
        {
            //ViewBag.BagEmpresas = EmpresaManager.FindAll<Empresa>();
            ViewBag.BagPerfis = _perfilManager.FindAll();
            ViewBag.ControllerAction = "Usuario";
            ViewBag.UrlRequestAction = "CadastroUsuario";
            ViewBag.UrlRequestController = "Usuario";
            return View();
        }

        /// <summary>
        /// Método que retorna a página Primeiro Acesso
        /// </summary>
        /// <returns></returns>
        public ActionResult ConsultaUsuario()
        {
            //Adiciona a lista de empresas na visao
            //ViewBag.BagEmpresas = EmpresaManager.FindAll<Empresa>();
            //Adicicona a lista de perfis na visao
            ViewBag.BagPerfis = _perfilManager.FindAll();
            //Adiciona a lista de statuis na visão
            ViewBag.BagStatus = _statusManager.FindAll();
            //Chama a pagina consulta usuario
            return View();
        }

        /// <summary>
        /// Método para inserir o usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsereUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                //everything is good. Lets save and redirect
                ViewBag.BagPerfis = _perfilManager.FindAll();
                //Adiciona o Perfil Solicitando ao usuario(Perfil Padrão)
                InsereUsuarioNaBase(usuario);
                ViewBag.UrlRequestAction = "CadastroUsuario";
                ViewBag.UrlRequestController = "Usuario";
            }
            //ViewBag.BagEmpresas = EmpresaManager.FindAll<Empresa>();
            ViewBag.BagPerfis = _perfilManager.FindAll();
            ViewBag.ControllerAction = "Usuario";
            ViewBag.UrlRequestAction = "CadastroUsuario";
            ViewBag.UrlRequestController = "Usuario";

            return View("CadastroUsuario");

        }
        /// <summary>
        /// Método que filtra os usuarios
        /// </summary>
        /// <param name="filtro">Filtro da tela ConsultaUsuario</param>
        /// <returns>
        ///     Retorna uma parcial com os usuarios filtrados
        /// </returns>
        [HttpPost]
        public PartialViewResult FiltraUsuarios(FiltroUsuario filtro)
        {
            //if (filtro.Dsc_cpf != null)
            //{
            //    filtro.Dsc_cpf = CpfUtil.LimpaCarateresCpf(filtro.Dsc_cpf);
            //}
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilter = _usuarioManager.AdicionaFiltros(filtro);
            IList<Usuario> usuarios = _usuarioManager.FindByFilter(fieldsFilter);
            return PartialView("_UsuariosFiltrados", usuarios);
        }

        /// <summary>
        /// Valida se os filtros passados pelo usuario são válidos
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidaFiltros(FiltroUsuario filtro)
        {
            //Verifica se o cpf é valido
            if (!ModelState.IsValidField("Dsc_cpf"))
            {
                return Json((int)EnumResultJson.ERRO_CPF_INVALIDO);
            }
            //Verifica se o nome é valido
            if (!ModelState.IsValidField("Dsc_nome"))
            {
                return Json((int)EnumResultJson.ERRO_NOME_EXCEDEU_TAMANHO_LIMITE);
            }
            //Verifica se o login é valido
            if (!ModelState.IsValidField("Dsc_login"))
            {
                return Json((int)EnumResultJson.ERRO_LOGIN_EXCEDEU_TAMANHO_LIMITE);
            }
            //if (filtro.Dsc_cpf != null)
            //{
            //    filtro.Dsc_cpf = CpfUtil.LimpaCarateresCpf(filtro.Dsc_cpf);
            //}
            // Preenche o objeto filtro
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilter = _usuarioManager.AdicionaFiltros(filtro);
            //Verifica se pelo menos um filtro foi preenchido
            if (fieldsFilter.Count.Equals(Constantes.EMPTY))
            {
                //Retorna codigo de erro
                return Json((int)EnumResultJson.ERRO_PELO_MENOS_UM_CAMPO_E_NECESSARIO_PARA_A_CONSULTA);
            }
            //Pega todos os usuarios pelo filtro
            IList<Usuario> usuarios = _usuarioManager.FindByFilter(fieldsFilter);
            //Verifica se a lista de usuarios está vazia
            if (usuarios.Count.Equals(Constantes.EMPTY))
            {
                //Retorna codigo de erro
                return Json((int)EnumResultJson.ERRO_DADOS_NAO_ENCONTRADOS);
            }
            //Retorno de Sucesso
            return Json((int)EnumResultJson.SUCESSO);
        }


        /// <summary>
        /// Chama a tela de manutenção
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public ActionResult Manutencao(int usuario)
        {
            //Pega o feriado passado por parâmetro
            Usuario usuarioBanco = _usuarioManager.FindByPk(usuario);
            CarregaCombos(usuarioBanco);
            return View(usuarioBanco);
        }

        /// <summary>
        /// Altera o Feriado
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="hdnAlterarExcluirUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlteraUsuario(Usuario usuario, string hdnAlterarExcluirUsuario)
        {
            
            CarregaCombos(usuario);

            if (!string.IsNullOrEmpty(hdnAlterarExcluirUsuario))
            {

            //Limpa caracteres do cpf
                if (hdnAlterarExcluirUsuario == "Alterar")
                {

                    //Limpa caracteres do cpf
            usuario.Dsc_cpf = CpfUtil.LimpaCarateresCpf(usuario.Dsc_cpf);

                    Usuario usuarioBase = _usuarioManager.GetUsuarioPeloCpf(usuario.Dsc_cpf);

                    NetUtil.Util.Hibernate.HibernateUtil.CloseSession();

                    if (usuarioBase != null)
                    {
                        if (usuarioBase.Equals(usuario))
                        {
                            AlteraUsuarioNaBase(usuario);
                        }
                        else
                        {
                            ModelState.AddModelError("usuario.Cpf", ViewMessagesResource.common_error_cpf_cadastrado);
                            return View("Manutencao", usuario);
                        }
                    }

                }
                else
                {
                    DeletaUsuario(usuario);
                    ViewBag.Status_Exclusao = true;
                    return View("Manutencao");
                }
            }
            else
            {
                try
                {
                    return View("Manutencao", usuario);
                }
                catch (Exception ex)
                {
                    return View("Error", ex);
                }
            }
            try
            {
                return View("Manutencao");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        /// <summary>
        /// Método que deleta o Feriado
        /// </summary>
        /// <returns></returns>
        public void DeletaUsuario(Usuario usuario)
        {
            _usuarioManager.Delete(usuario);
        }

        /// <summary>
        /// Método que valida se é necessário mandar e-mail, logo após a validação, altera o usuario na base.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="produtos"></param>
        /// <param name="tipoSolicitacao"></param>
        //private void AlteraUsuarioNaBase(Usuario usuario, int[] produtos, int[] tipoSolicitacao)
        private void AlteraUsuarioNaBase(Usuario usuario)
        {
            //Preenche o objeto usuario com os produtos e tiposolicitacao escolhidos pelo administrador
            //PreencheUsuarioComProdutosETipoSolicitacao(usuario, produtos, tipoSolicitacao);
            //Verifica se é necessário enviar email para o usuario
            if (usuario.Status.Id_status.Equals((int)EnumStatus.ATIVO) && usuario.Dsc_senha == null)
            {
                //Gera um senha
                string senha = PasswordUtils.GerarSenha();
                //Criptografa a senha
                usuario.Dsc_senha = EncryptionUtils.SHA256Hash(senha);
                //Envia email
                EnviarEmail((int)EnumEmailTipo.ACESSO_LIBERADO, usuario, senha);
            }
            //Faz update 
            _usuarioManager.SaveOrUpdate(usuario);
            //Faz sinal para enviar mensagem de sucesso
            ViewBag.Status_Alteracao = true;
        }

        public ActionResult ConsultaPerfilAcesso()
        {
            ViewBag.ControllerAction = "Usuario";
            ViewBag.UrlRequestAction = "ConsultaPerfilAcesso";
            ViewBag.UrlRequestController = "Usuario";
            ViewBag.BagPerfis = _perfilManager.FindAll();
            var session = HibernateUtil.GetCurrentSession();
            var query = from p in session.Query<FluxoAtendimento>()
                        select p;
            ViewBag.BagFluxos = query.ToList();
            HibernateUtil.CloseSession();
            return View("ConsultarPerfilAcesso");
        }

        /// <summary>
        /// Retorna Json da Consulta de Perfil e Fluxo do usuario
        /// </summary>
        /// <returns></returns>
        public JsonResult carregaPerfilFluxoUsuario()
        {
            try
            {
                var login = Request["login"];
                var id_perfil = Request["id_perfil"];
                var id_fluxo_atendimento = Request["id_fluxo_atendimento"];

                var session = HibernateUtil.GetCurrentSession();

                var query = from p in session.Query<Usuario>()
                            join c in session.Query<TbUsuarioPerfil>()
                            on p.Id_usuario equals c.TbUsuario.Id_usuario
                            join x in session.Query<TbPerfilFluxoAtendimento>()
                            on c.CodigoPerfil equals x.TbPerfil.Id_perfil
                            join q in session.Query<FluxoAtendimento>()
                            on x.TbFluxoAtendimento.Codigo equals q.Codigo
                            join per in session.Query<Perfil>()
                            on c.CodigoPerfil equals per.Id_perfil
                            where p.Dsc_login == login
                            select new { p, q  ,per};


                if (!string.IsNullOrEmpty(id_perfil))
                {
                    var _id_perfil = Convert.ToInt32(id_perfil);
                    query = query.Where(x => x.p.Perfils.First().Id_perfil == _id_perfil);
                }

                if (!string.IsNullOrEmpty(id_fluxo_atendimento))
                {
                    var _id_fluxo_atendimento = Convert.ToInt32(id_fluxo_atendimento);
                    query = query.Where(x => x.q.Codigo == _id_fluxo_atendimento);
                }

                var lista = new List<DTO_Consulta_Perfil_Fluxo_Atendiemnto>();

                foreach (var item in query)
                {
                    var consulta = new DTO_Consulta_Perfil_Fluxo_Atendiemnto()
                    {
                        id_fluxo_Atendimento = item.q.Codigo,
                        id_perfil = item.per.Id_perfil ?? 0,
                        nome_usuario = item.p.Dsc_login,
                        desc_fluxo_Atendimento = item.q.DescricaoFluxoAtendimento,
                        desc_perfil = item.per.Dsc_descricao
                    };

                    lista.Add(consulta);
                }

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                RedirectToAction("ErrorLogin", ex);
                return Json("erro", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult AlterarPerfilUsuario() 
        {
            if (Session[Constantes.SESSAO_DO_USUARIO] != null) 
            {
                SGCA.Models.DTO.SessaoDoUsuario sdu = (SGCA.Models.DTO.SessaoDoUsuario)@Session[Constantes.SESSAO_DO_USUARIO];

                var login = sdu.Login;

                var session = HibernateUtil.GetCurrentSession();

                var query = from p in session.Query<Usuario>()
                            join c in session.Query<TbUsuarioPerfil>()
                            on p.Id_usuario equals c.CodigoUsuario
                            where p.Dsc_login == login
                            && c.CodigoPerfil == 1
                            select p;

                if (query.Any()) 
                {
                    var usu = Request["login_consulta"];
                    ViewBag.Nome_usuario = usu;

                    var query7 = from p in session.Query<Usuario>()
                                where p.Dsc_login == login
                                select p.Id_usuario;
                    ViewBag.id_Usu = query7.FirstOrDefault();
                    ViewBag.BagPerfis = _perfilManager.FindAll();
                    var query2 = from p in session.Query<FluxoAtendimento>()
                                select p;

                    ViewBag.BagFluxos = query2.ToList();
                    ViewBag.Nome_usuario = query.FirstOrDefault().Dsc_login;
                    return View("CadastrarPerfilAcesso");
                }
                else 
                {
                    RedirectToAction("ConsultaPerfilAcesso", new RouteValueDictionary(new { controller = "UsuarioController", action = "ConsultaPerfilAcesso", Id = "acesso=0" }));
                    return View("CadastrarPerfilAcesso");
                }

            }
            else 
            {
                RedirectToAction("ConsultaPerfilAcesso", new RouteValueDictionary(new { controller = "UsuarioController", action = "ConsultaPerfilAcesso", Id = "acesso=0" }));
                return View("CadastrarPerfilAcesso");
            }
        }



        public JsonResult alteraPerfilFluxoUsu()
        {
            try
            {
                var id_usuario = Request["id_usu"];
                var id_fluxo = Request["id_fluxo"];
                var id_perfis = Request["idsperfils"];

                var vet = id_perfis.Split('|').ToList().Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                var _id_usuario = Convert.ToInt32(id_usuario);
                var _id_fluxo = Convert.ToInt32(id_fluxo);
       
                var session = HibernateUtil.GetCurrentSession();

                using (var tx = session.BeginTransaction())
                {
                    //deletando anteriores
                    var del1 = from p in session.Query<TbUsuarioPerfil>()
                               where p.CodigoUsuario == _id_usuario
                               select p;

                    foreach (var item in del1)
                    {
                        session.Delete(item);
                    }

                    //inserindo os novos
                    foreach (var item in vet)
                    {
                        var id_perfil = Convert.ToInt32(item);

                        var del2 = from p in session.Query<TbPerfilFluxoAtendimento>()
                                   where p.TbPerfil.Id_perfil == id_perfil
                                   select p;

                        if (del2.Any()) 
                        {
                            foreach (var item1 in del2)
                            {
                                session.Delete(item1);
                            }
                        }
                    }

                    tx.Commit();
                }

                var TbFluxoAtendimento = (from p in session.Query<FluxoAtendimento>()
                                          where p.Codigo == _id_fluxo
                                          select p).FirstOrDefault();
                
                foreach (var item in vet)
                {
                     var id_perfil = Convert.ToInt32(item);
                     var sql = "INSERT INTO TB_USUARIO_PERFIL VALUES (" + _id_usuario + "," + id_perfil + ")";
                     var sql2 = "INSERT INTO TB_PERFIL_FLUXO_ATENDIMENTO VALUES (" + _id_fluxo + "," + id_perfil + ")";
                     session.CreateSQLQuery(sql).ExecuteUpdate();
                     session.CreateSQLQuery(sql2).ExecuteUpdate();             
                }


                var json = new DTO_JSON_Result()
                {
                    status = 1,
                    msg = "{OK}"
                };
                return Json(json, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var json = new DTO_JSON_Result()
                {
                    status = 0,
                    msg = ex.InnerException.ToString()
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
