using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Util
{
    public class Constantes
    {
        public static char CSV_CHAR_SPLIT = ';';

        public const string SESSAO_DO_USUARIO = "sessaodousuario";
        public const string SESSOES_DOS_USUARIOS = "sessoesdosusuarios";

        //VIEWS
        public const string VIEW_ERRO = "Error";
        public const string VIEW_LOGIN = "Login";

        //FiltroAutorizacao
        public const string CONTROLLER_HOME = "Home";
        public const string CONTROLLER_LOGIN = "Login";
        public const string ACTION_ERRO_ACESSO = "ErroAcesso";
        public const string ACTION_LOGIN_SIMULTANEO = "LoginSimultaneo";
        public const string ACTION_LOGIN = "Login";
        public const char SEPARADOR = '/';

        //Importacao
        public const string IMPORTADADOS = "ImportaDados";
        public const string VERIFICALAYOUTARQUIVO = "VerificaLayoutArquivo";

        //Entities
        public const string REGULAR_EXPRESSION_EMAIL = @"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}";
        public const string REGULAR_EXPRESSION_SENHA = @"^[a-zA-Z][0-9A-Za-z@!#$%¨&*()_><?/}{§|\.]*\d+[0-9A-Za-z@!#$%¨&*()_><?/}{§|\.]*$";

        public const string PASTA_AGUARDANDO = "/aguardando";
        public const string PASTA_SUCESSO = "/sucesso";
        public const string PASTA_FALHA = "/falha";

        public const string APP_CONFIG_PASTA_RAIZ = "pastaraiz";
        public const string APP_CONFIG_TAMANHO_PAGINA = "tamanhopagina";
        public const string APP_CONFIG_ATUALIZACAO_PAGINA_IMPORTACAO = "atualizacaopaginaimportacao";
        public const string USUARIO_SISTEMA_BATCH = "BATCH";
        ///antigo

        public const int EMPTY = 0;

        /*PASTA ANEXOS DEVENVOLVIMENTO LOCAL*/
        protected const string LOCAL_ARQUIVOS_TEMPORARIOS = "../tempData/";
        /*FIM PASTA ANEXOS DESENVOLVIMENTO LOCAL*/

        // PASTA SFTP //
        protected const string SFTP_ANEXO = "anexos/";
        protected const string SFTP_ANEXO_CARTEIRA = "anexos/carteira/";
        protected const string SFTP_ANEXO_ATIS = "anexos/atis/";
        protected const string SFTP_ANEXO_LEGADO = "anexos/legado/";
        protected const string SFTP_PLANILHAS_MODELO = "anexos/planilhaModeloCredito/";
        protected const string SFTP_ANEXO_SOLICITACAO_VALIDACAO = "anexos/validacaosolicitacao/";
        protected const string SFTP_ANEXO_SOLICITACAO_BAIXA_COBRANCA = "anexos/solicitacaobaixacobranca/";
        // FIM PASTA SFTP//

        /*PASTA ANEXOS HOMOLOGACAO*/
        //private const string LOCAL_ANEXO = "/SGCA/anexos/";
        //private const string LOCAL_ANEXO_SOLICITACAO_BAIXA_COBRANCA = "/SGCA/anexos/solicitacaobaixacobranca";
        /*FIM PASTA ANEXOS HOMOLOGACAO*/

        ///
    }
}
