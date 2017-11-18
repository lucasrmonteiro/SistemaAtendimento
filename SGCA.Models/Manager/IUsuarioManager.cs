using SGCA.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGCA.Models.Filters;
using NetUtil.Util.Enums;
using SGCA.Models.Manager.Base;

namespace SGCA.Models.Manager
{
    public interface IUsuarioManager : IBaseManager<Usuario>
    {
        /// <summary>
        ///     Método que verifica se o cpf já existe na base
        /// </summary>
        /// <param name="cpf">Cpf do usuario</param>
        /// <returns>
        ///     True - Se o cpf existir
        ///     False - Se o cpf não existir
        /// </returns>
        bool VerificaSeCpfExisteNaBase(string cpf);

        /// <summary>
        ///     Método que retorna o usuário com o cpf passado por parâmetro.
        /// </summary>
        /// <param name="cpf">Cpf do usuário</param>
        /// <returns>
        ///     Usuário com o cpf passado por parâmetro
        /// </returns>
        Usuario GetUsuarioPeloCpf(string cpf);
        
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
        Usuario BuscaUsuarioPorLogin(string login);

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
        Usuario AtualizaSenha(string login, string novaSenha);

        /// <summary>
        ///     Pega a lista de email dos administradores
        /// </summary>
        /// <returns>
        ///     Lista de email dos administradores
        /// </returns>
        IList<string> GetListaDeEmailDosAdministradores();

        /// <summary>
        /// Adiciona filtros para consulta
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        IDictionary<string, IDictionary<Restriction, object>> AdicionaFiltros(FiltroUsuario filtro);

        void InsereUsuarioNaBase(Usuario usuario);

        void SaveOrUpdate(Usuario usuario);
        
        void Delete(Usuario u);

        Usuario FindByPk(int usuario);

        IList<Usuario> FindByFilter(IDictionary<string, IDictionary<Restriction, object>> fieldsFilter);
    }
}
