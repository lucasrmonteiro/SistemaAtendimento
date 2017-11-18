using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Manager.Exceptions
{
    public class ImportacaoIniciadaPeloSistemaException : ManagerException
    {
        public ImportacaoIniciadaPeloSistemaException()
            : base(Resources.mensagem_info_importacao_indexada_sistema)
        {
        }
    }
}
