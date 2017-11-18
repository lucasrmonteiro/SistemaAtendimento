using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Manager.Exceptions
{
    public class ImportacaoIndexadaPeloSistemaException : ManagerException
    {
        public ImportacaoIndexadaPeloSistemaException()
            : base(Resources.mensagem_info_importacao_indexada_sistema)
        {
        }
    }
}
