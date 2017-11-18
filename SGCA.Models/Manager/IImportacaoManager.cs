using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGCA.Models.Entity;
using SGCA.Models.Manager.Base;
using NetUtil.Util.DTO;

namespace SGCA.Models.Manager
{
    public interface IImportacaoManager : IBaseManager<Importacao>
    {
        IList<string> GetArquivosImportacao();

        DataTableData GetImportacoes(int pageSize, int currentPage, int idUsuario);

        void AdicionaExecutaImportacao(Importacao importacao);

        void ImportaArquivos();
    }
}