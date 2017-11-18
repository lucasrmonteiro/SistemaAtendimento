using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Manager.Exceptions
{
    public class LinhaArquivoInvalidoException : ManagerException
    {
        public LinhaArquivoInvalidoException()
            : base(Resources.mensagem_erro_layout_arquivo_invalido)
        {
        }

        public LinhaArquivoInvalidoException(Exception ex)
            : base(Resources.mensagem_erro_layout_arquivo_invalido, ex)
        {
        }

        public LinhaArquivoInvalidoException(string message)
            : base(message)
        {
        }
    }
}
