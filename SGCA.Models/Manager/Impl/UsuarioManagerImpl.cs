using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGCA.Models.Entity;
using SGCA.Models.Enums;
using SGCA.Models.Filters;
using SGCA.Models.Manager.Base.Impl;
using NetUtil.Util.Enums;
using SGCA.Models.Util;
using SGCA.Models.DAO;
using NetUtil.Util.Hibernate;

namespace SGCA.Models.Manager.Impl
{
    public class UsuarioManagerImpl : BaseManagerImpl<Usuario>, IUsuarioManager
    {

        public UsuarioManagerImpl(IGenericDAO dao)
        {
            this._dao = dao;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public bool VerificaSeCpfExisteNaBase(string cpf) 
        {
            return _dao.FindEntityByFilter<Usuario>(new Dictionary<string, object>() { { "Dsc_cpf", cpf } }, null) == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public Usuario GetUsuarioPeloCpf(string cpf)
        {
            return _dao.FindEntityByFilter<Usuario>(new Dictionary<string, object>() { { "Dsc_cpf", cpf } }, null);
        }

        /// <summary>
        ///     Método que busca um usuário no banco usando o 'login'.
        /// </summary>
        /// <param name="login">
        ///     String para busca do usuário no banco.
        /// </param>
        /// <returns>
        ///     Se encontrar, retorna o primeiro 'Usuario' encontrado.
        ///     Caso contrário, retorna 'null'.
        /// </returns>
        public Usuario BuscaUsuarioPorLogin(string login)
        {
            //Cria um filtro de campos
            IDictionary<string, object> fieldsFilter = 
                new Dictionary<string, object>();
            
            //Adiciona o campo 'Dsc_login' ao filtro
            fieldsFilter.Add("Dsc_login", login);

            //Busca usuários no banco, filtrando por 'Dsc_login'
            IList<Usuario> usuarios = _dao.FindByFilter<Usuario>(fieldsFilter, null);

            return usuarios.Count.Equals(0) ? null : usuarios[0];
        }

        /// <summary>
        ///     Atualiza senha do usuário no banco.
        ///     Busca usuário na base utilizando o login
        ///     e depois atualiza a senha.
        /// </summary>
        /// <param name="login">
        ///     String com o login do usuário a ser buscado.
        /// </param>
        /// <param name="novaSenha">
        ///     String com a nova senha para ser atualizada no banco.
        /// </param>
        /// <returns>
        ///     Sucesso: Objeto Usuario.
        ///     Falha: null.
        /// </returns>
        public Usuario AtualizaSenha(string login, string novaSenha)
        {
            Usuario usuario = BuscaUsuarioPorLogin(login);

            if (usuario != null)
            {
                usuario.Dsc_senha = novaSenha;
                usuario.primeiro_acesso = false;
                _dao.Alterar(usuario); 
            }

            return usuario;
        }

        /// <summary>
        ///     Pega a lista de email dos administradores
        /// </summary>
        /// <returns>
        ///     Lista de email dos administradores
        /// </returns>
        public IList<string> GetListaDeEmailDosAdministradores()
        {
            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, object> fieldsFilter = new Dictionary<string, object>();

            // Adiciona o filtro de perfil
            fieldsFilter.Add("Perfil.Id_perfil", (int)EnumPerfil.ADMINISTRADOR);
            //Busca usuarios pelo filtro
            IList<Usuario> usuarios = _dao.FindByFilter<Usuario>(fieldsFilter, null);
            //cria uma lista de emails
            IList<string> emails = new List<string>();
            //preenche a lista de emails com os emails dos administradores
            foreach (Usuario usuario in usuarios)
            {
                emails.Add(usuario.Dsc_email);
            }
            //retorna lista de emails
            return emails;
        }

        /// <summary>
        /// Adiciona filtros para consulta
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public IDictionary<string, IDictionary<Restriction, object>> AdicionaFiltros(FiltroUsuario filtro)
        {
            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, object> fieldsFilter = new Dictionary<string, object>();

            // Cria o filtro para realizar a pesquisa dos dados
            IDictionary<string, IDictionary<Restriction, object>> fieldsFilterWithRestriction = new Dictionary<string, IDictionary<Restriction, object>>(); 

            if (!String.IsNullOrEmpty(filtro.Dsc_cpf))
            {
                IDictionary<Restriction, object> filtroCPF = new Dictionary<Restriction, object>();
                filtroCPF.Add(Restriction.Eq, filtro.Dsc_cpf);
                fieldsFilterWithRestriction.Add("Dsc_cpf", filtroCPF);
            }

            if (!String.IsNullOrEmpty(filtro.Dsc_login))
            {
                IDictionary<Restriction, object> filtroLogin = new Dictionary<Restriction, object>();
                filtroLogin.Add(Restriction.LikeRight, filtro.Dsc_login);
                fieldsFilterWithRestriction.Add("Dsc_login", filtroLogin);
            }

            if (!String.IsNullOrEmpty(filtro.Dsc_nome))
            {
                IDictionary<Restriction, object> filtroNome = new Dictionary<Restriction, object>();
                filtroNome.Add(Restriction.LikeRight, filtro.Dsc_nome);
                fieldsFilterWithRestriction.Add("Dsc_nome", filtroNome);
            }

            if (filtro.Empresa != 0)
            {
                IDictionary<Restriction, object> filtroEmpresa = new Dictionary<Restriction, object>();
                filtroEmpresa.Add(Restriction.Eq, filtro.Empresa);
                fieldsFilterWithRestriction.Add("Empresa.Id_empresa", filtroEmpresa);
            }

            if (filtro.Perfil != 0)
            {
                IDictionary<Restriction, object> filtroPerfil = new Dictionary<Restriction, object>();
                filtroPerfil.Add(Restriction.Eq, filtro.Perfil);
                fieldsFilterWithRestriction.Add("Perfil.Id_perfil", filtroPerfil);
            }

            if (filtro.Status != 0)
            {
                IDictionary<Restriction, object> filtroStatus = new Dictionary<Restriction, object>();
                filtroStatus.Add(Restriction.Eq, filtro.Status);
                fieldsFilterWithRestriction.Add("Status.Id_status", filtroStatus);
            }

            return fieldsFilterWithRestriction;
        }

        public void InsereUsuarioNaBase(Usuario usuario)
        {
            //Converte o cpf da view para o cpf que será serializado
            //usuario.Dsc_cpf = CpfUtil.LimpaCarateresCpf(usuario.Dsc_cpf);

            _dao.Incluir<Usuario>(usuario);
            var session = HibernateUtil.GetCurrentSession();
            var sql = "INSERT INTO TB_USUARIO_PERFIL VALUES (" + usuario.Id_usuario + "," + usuario.Perfil.Id_perfil + ")";
            session.CreateSQLQuery(sql).ExecuteUpdate();
        }


        public void SaveOrUpdate(Usuario usuario)
        {
            _dao.SaveOrUpdate<Usuario>(usuario);
        }


        public void Delete(Usuario usuario)
        {
            Usuario u = new Usuario();
            u = _dao.FindByPK<Usuario>(usuario.Id_usuario);
            u.FlAtivo = false;
            _dao.SaveOrUpdate<Usuario>(u);
        }

        public Usuario FindByPk(int pk)
        {
            return _dao.FindByPK<Usuario>(pk);
        }

        public IList<Usuario> FindByFilter(IDictionary<string, IDictionary<Restriction, object>> fieldsFilter)
        {
            return _dao.FindByFilter<Usuario>(fieldsFilter, null);
        }
    }
}