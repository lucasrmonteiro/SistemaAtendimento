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
using System.Configuration;
using System.IO;
using NHibernate;

namespace SGCA.Models.DTO
{
    public class SessaoDoUsuario
    {
        private Usuario _usuario;

        private string _login;
        private string _nome;

        //private IList<string> _funcoes;

        private IList<TbUsuarioPerfil> _perfil;

        public SessaoDoUsuario(Usuario u)
        {
            _usuario = u;
            _login = u.Dsc_login;
            _nome = u.Dsc_nome;
            var session = HibernateUtil.OpenSession();
            var query = (from p in session.Query<TbUsuarioPerfil>()
                         where p.TbUsuario == u
                         select p).ToList();
            _perfil = query;
            //_funcoes = new List<string>();
            //foreach (var f in u.Perfil.Funcoes)
            //{
            //    _funcoes.Add(f.ToString());
            //}
        }

        public Usuario Usuario
        {
            get { return _usuario; }
        }

        public IList<TbUsuarioPerfil> Perfil
        {
            get { return _perfil; }
        }


        public string Login
        {
            get { return _login; }
        }
        public string Nome
        {
            get { return _nome; }
        }

        protected bool IsAnalistaNivel1()
        {
            return _perfil.Equals((int)EnumPerfil.ANALISTA_NIVEL1) ? true : false;
        }

        protected bool IsAnalistaNivel2()
        {
            return _perfil.Equals((int)EnumPerfil.ANALISTA_NIVEL2) ? true : false;
        }

        protected bool IsAnalistaNivel3()
        {
            return _perfil.Equals((int)EnumPerfil.ANALISTA_NIVEL3) ? true : false;
        }

        public bool TemPermissao(string controller, string action)
        {
            string strFuncao = Funcao.ToString(controller, action);
            return true;//_funcoes.Contains(strFuncao);
        }
    }
}